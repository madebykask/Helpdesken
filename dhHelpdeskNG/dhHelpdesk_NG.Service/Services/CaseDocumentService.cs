using DH.Helpdesk.BusinessData.Models.CaseSolution;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Repositories.CaseDocument;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;
    using System.Reflection;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Enums;

    public interface ICaseDocumentService
    {
        CaseDocumentModel GetCaseDocument(int id);
        IEnumerable<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType);
    }

    public class CaseDocumentService : ICaseDocumentService
    {
        private readonly ICaseDocumentRepository _caseDocumentRepository;
        private readonly ICaseDocumentConditionRepository _caseDocumentConditionRepository;
        private readonly ICacheProvider _cache;
        private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;

        public CaseDocumentService(
            ICaseDocumentRepository caseDocumentRepository,
            ICaseDocumentConditionRepository caseDocumentConditionRepository,
            ICacheProvider cache,
            IExtendedCaseValueRepository extendedCaseValueRepository
            )
        {
            this._caseDocumentRepository = caseDocumentRepository;
            this._caseDocumentConditionRepository = caseDocumentConditionRepository;
            this._cache = cache;
            this._extendedCaseValueRepository = extendedCaseValueRepository;
        }

        public CaseDocumentModel GetCaseDocument(int id)
        {
            return this._caseDocumentRepository.GetCaseDocument(id);
        }
      
        public IEnumerable<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType)
        {
            //TODO, do not need to fetch all from customer, filter this out better
            var caseDocuments = this._caseDocumentRepository.GetCaseDocumentsByCustomer(customerId).Select(c => new CaseDocumentModel()
            {
                Id = c.Id,
                Name = c.Name,
                FileType = c.FileType,
                SortOrder = c.SortOrder,
            }).OrderBy(c => c.SortOrder).ToList();


            return caseDocuments.Where(c => show(customerId, _case, c.Id, user, applicationType) == true).ToList();

        }

        //TODO: REFACTOR, this could be used the "same" way as workflowstep... 
        private bool show(int customerId, Case _case, int caseDocumentId, UserOverview user, ApplicationType applicationType)
        {
            //ALL conditions must be met
            bool showDocument = false;

            //If no conditions are set in (CaseSolutionCondition) for the template, do not show step in list
            var caseSolutionConditions = this.GetCaseDocumentConditions(caseDocumentId);

            if (caseSolutionConditions == null || caseSolutionConditions.Count() == 0)
                return false;

            foreach (var condition in caseSolutionConditions)
            {
                var conditionValue = condition.Value.Tidy().ToLower();
                var conditionKey = condition.Key.Tidy();

                try
                {
                    var value = "";

                    //GET FROM APPLICATION
                    if (conditionKey.ToLower() == "application_type")
                    {
                        int appType = (int)((ApplicationType)Enum.Parse(typeof(ApplicationType), applicationType.ToString()));
                        value = appType.ToString();
                    }
                    //GET FROM EXTENDEDCASE
                    else if (conditionKey.ToLower().StartsWith("extendedcase_"))
                    {
                        conditionKey = conditionKey.Replace("extendedcase_", "");
                        value = _extendedCaseValueRepository.GetExtendedCaseValue(_case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData_Id, conditionKey).Value;
                    }
                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_"))
                    {
                        conditionKey = conditionKey.Replace("user_", "");
                        //Get value from Model by casting to dictionary and look for property name
                        value = user.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(user, null))[conditionKey].ToString();
                    }
                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        conditionKey = conditionKey.Replace("case_", "");

                        if (!conditionKey.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[conditionKey].ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                            var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[parentClassName];
                            value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[propertyName].ToString();
                        }
                    }
                    
                    // Check conditions
                    if (value.Length > 0 && conditionValue.IndexOf(value.ToLower()) > -1)
                    {
                        showDocument = true;
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                    //TODO?
                    return false;
                }
            }

            //To be true all conditions needs to be fulfilled
            return showDocument;
        }

        public Dictionary<string, string> GetCaseDocumentConditions(int caseDocument_Id)
        {
            Dictionary<string, string> caseDocumentConditions = _caseDocumentConditionRepository.GetCaseDocumentConditions(caseDocument_Id).Select(x => new { x.Property_Name, x.Values }).ToDictionary(x => x.Property_Name, x => x.Values);
            return caseDocumentConditions;
        }
    }
}
