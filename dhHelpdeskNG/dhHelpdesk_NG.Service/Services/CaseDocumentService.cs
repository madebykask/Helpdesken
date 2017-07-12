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
        CaseDocumentModel GetCaseDocument(int id, int caseId);
        IEnumerable<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType);
    }

    public class CaseDocumentService : ICaseDocumentService
    {
        private readonly ICaseDocumentRepository _caseDocumentRepository;
        private readonly ICaseDocumentConditionRepository _caseDocumentConditionRepository;
        private readonly ICacheProvider _cache;
        private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;
        private readonly ICaseService _caseService;

        public CaseDocumentService(
            ICaseDocumentRepository caseDocumentRepository,
            ICaseDocumentConditionRepository caseDocumentConditionRepository,
            ICacheProvider cache,
            IExtendedCaseValueRepository extendedCaseValueRepository,
             ICaseService caseService
            )
        {
            this._caseDocumentRepository = caseDocumentRepository;
            this._caseDocumentConditionRepository = caseDocumentConditionRepository;
            this._cache = cache;
            this._extendedCaseValueRepository = extendedCaseValueRepository;
            this._caseService = caseService;
        }

        private string GetCaseValue(Case _case, string propertyName)
        {
            string value = string.Empty;

            try
            {
                
                propertyName = propertyName.Replace("Case.", "");

                if (!propertyName.Contains("."))
                {
                    //Get value from Case Model by casting to dictionary and look for property name
                    value = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[propertyName].ToString();
                }
                else
                {
                    var parentClassName = string.Concat(propertyName.TakeWhile((c) => c != '.'));
                    var parentPropertyName = propertyName.Substring(propertyName.LastIndexOf('.') + 1);

                    var parentClass = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[parentClassName];
                    value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[parentPropertyName].ToString();
                }

            }
            catch (Exception)
            {
                //Return back propertyname
                value = "[" + propertyName + "]";
            }

            return value;
        }

        private Dictionary<string,string> GetCaseValueDictionary(int id, int caseId)
        {
            var _case = _caseService.GetCaseById(caseId);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            #region Case

            string casePersonsName = "Case.PersonsName";
            dictionary.Add(casePersonsName, GetCaseValue(_case, casePersonsName));

            string casePersonsPhone = "Case.PersonsPhone";
            dictionary.Add(casePersonsPhone, GetCaseValue(_case, casePersonsPhone));

            string caseDepartmentName = "Case.Department.DepartmentName";
            dictionary.Add(caseDepartmentName, GetCaseValue(_case, caseDepartmentName));

            string caseRegionName = "Case.Region.Name";
            dictionary.Add(caseRegionName, GetCaseValue(_case, caseRegionName));
           

            #endregion

            #region Other

            //TODO: do we neeed to convert?
            string dateNowLong = "Date.NowLong";
            dictionary.Add(dateNowLong, DateTime.Now.ToLongDateString());

            string dateNowShort = "Date.NowShort";
            dictionary.Add(dateNowShort, DateTime.Now.ToShortDateString());


            #endregion

            #region Extended Case

            //[PositionTitle]
            dictionary.Add("[PositionTitle]", "[PositionTitle]");

            //[ReportsToLineManager.PositionTitle]
            dictionary.Add("[ReportsToLineManager.PositionTitle]", "[ReportsToLineManager.PositionTitle]");

            //[ReportsToLineManager]
            dictionary.Add("[ReportsToLineManager]", "[ReportsToLineManager]");



            //[AddressLine1]
            dictionary.Add("[AddressLine1]", "[AddressLine1]");
            //[AddressLine2]
            dictionary.Add("[AddressLine2]", "[AddressLine2]");
            //[State]
            dictionary.Add("[State]", "[State]");
            //[PostalCode]
            dictionary.Add("[PostalCode]", "[PostalCode]");
            //[AddressLine3]
            dictionary.Add("[AddressLine3]", "[AddressLine3]");
            //[ContractStartDate]
            dictionary.Add("[ContractStartDate]", "[ContractStartDate]");
            //[BasicPayAmount]
            dictionary.Add("[BasicPayAmount]", "[BasicPayAmount]");
            //[ContractedHours]
            dictionary.Add("[ContractedHours]", "[ContractedHours]");
            //[NextSalaryReviewYear]
            dictionary.Add("[NextSalaryReviewYear]", "[NextSalaryReviewYear]");

            #endregion

            return dictionary;

        }

        private string ReplaceWithValue(int id, int caseId, string text)
        {
            Dictionary<string, string> dictionary = GetCaseValueDictionary(id, caseId);

            foreach (var item in dictionary)
            {
                text = text.Replace("[" + item.Key + "]", item.Value);
            }

            return text;
        }

        public CaseDocumentModel GetCaseDocument(int id, int caseId)
        {
            //var _case = _caseService.GetCaseById(caseId);


            var caseDocument = this._caseDocumentRepository.GetCaseDocument(id);

            //var firstName = "Case.PersonsName";

            //var value = GetCaseValue(_case, firstName);

            //var depName = "Case.Department.DepartmentName";

            //var valueDep = GetCaseValue(_case, depName);


            //Case.PersonsPhone

            if (caseDocument.CaseDocumentParagraphs != null)
            {
                foreach (var item in caseDocument.CaseDocumentParagraphs)
                {
                    if (item.CaseDocumentParagraph.CaseDocumentTexts != null)
                    {
                        foreach (var paragraphText in item.CaseDocumentParagraph.CaseDocumentTexts)
                        {
                            paragraphText.Text = ReplaceWithValue(id, caseId, paragraphText.Text);

                        }
                    }
                }
            }


            return caseDocument;
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
                CaseId = _case.Id,
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
