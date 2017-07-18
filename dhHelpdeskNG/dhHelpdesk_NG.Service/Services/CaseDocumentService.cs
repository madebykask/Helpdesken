using DH.Helpdesk.BusinessData.Models.CaseSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

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
        CaseDocumentModel GetCaseDocument(Guid caseDocumentGUID, int caseId);
        IEnumerable<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType);
    }

    public class CaseDocumentService : ICaseDocumentService
    {
        private readonly ICaseDocumentRepository _caseDocumentRepository;
        private readonly ICaseDocumentConditionRepository _caseDocumentConditionRepository;
        private readonly ICacheProvider _cache;
        private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;
        private readonly ICaseService _caseService;
        private readonly ICaseDocumentTextConditionRepository _caseDocumentTextConditionRepository;
        private readonly ICaseDocumentTextIdentifierRepository _caseCaseDocumentTextIdentifierRepository;

        //TODO: Move to DB
        private const string DateShortFormat = "dd.MM.yyyy";
        private const string DateLongFormat = "d MMMM yyyy";


        public CaseDocumentService(
            ICaseDocumentRepository caseDocumentRepository,
            ICaseDocumentConditionRepository caseDocumentConditionRepository,
            ICacheProvider cache,
            IExtendedCaseValueRepository extendedCaseValueRepository,
            ICaseService caseService,
            ICaseDocumentTextConditionRepository caseDocumentTextConditionRepository,
            ICaseDocumentTextIdentifierRepository caseCaseDocumentTextIdentifierRepository
            )
        {
            this._caseDocumentRepository = caseDocumentRepository;
            this._caseDocumentConditionRepository = caseDocumentConditionRepository;
            this._cache = cache;
            this._extendedCaseValueRepository = extendedCaseValueRepository;
            this._caseService = caseService;
            this._caseDocumentTextConditionRepository = caseDocumentTextConditionRepository;
            this._caseCaseDocumentTextIdentifierRepository = caseCaseDocumentTextIdentifierRepository;
        }


        private bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetExtendedCaseValue(Case _case, string fieldId, string displayName)
        {
            
            try
            {
                //Check if SecondaryValue exist
                var value = _case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData.ExtendedCaseValues.Where(x => x.FieldId.ToLower() == fieldId.ToLower()).First().SecondaryValue;

                //REFACTOR
                if (string.IsNullOrEmpty(value))
                {
                    //Check for value
                    value = _case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData.ExtendedCaseValues.Where(x => x.FieldId.ToLower() == fieldId.ToLower()).First().Value;

                    if (string.IsNullOrEmpty(value))
                    {
                        return "[" + displayName.Replace("<", "").Replace(">", "").Replace("[", "").Replace("]", "") + "]";
                    }
                }

                if (CheckDate(value))
                {
                    
                    //INPUT NEEDS TO BE like: 2017-07-26
                    DateTime convertedDate = DateTime.ParseExact(value, "d", null);
                    return convertedDate.ToString(DateShortFormat, CultureInfo.InvariantCulture);
                }

                return value;
            }
            catch (Exception ex)
            {
                return  "[" + displayName.Replace("<", "").Replace(">", "").Replace("[", "").Replace("]", "") + "]";
                 
            }
        }

        private string GetCaseValue(Case _case, string propertyName, string displayName)
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
                value = "[" + displayName.Replace("<", "").Replace(">","").Replace("[", "").Replace("]","") + "]";
            }

            return value;
        }

        private Dictionary<string, string> GetCaseValueDictionary(int id, int caseId)
        {
      
            var _case = _caseService.GetCaseById(caseId);

            //Get Identifiers for Case and for ExtendedCase that is connected

            int extendedCaseFormId = 0;
            if (_case.CaseExtendedCaseDatas != null)
            {
                extendedCaseFormId = _case.CaseExtendedCaseDatas.First().ExtendedCaseData.ExtendedCaseFormId;
            }

            var identifiers = _caseCaseDocumentTextIdentifierRepository.GetCaseDocumentTextIdentifiers(extendedCaseFormId);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in identifiers)
            {
                string propertyName = item.PropertyName;
                //TODO: Translate DisplayName from MasterData
                string displayName = (!string.IsNullOrEmpty(item.DisplayName) ? item.DisplayName : item.Identifier);

                if (item.ExtendedCaseFormId == 0)
                {

                    #region  Case or Date
                     if (item.PropertyName.ToLower().Contains("case"))
                    { 
                        dictionary.Add(item.Identifier, GetCaseValue(_case, propertyName, displayName));
                    }
                    else if (item.PropertyName == "Date.NowLong")
                    {
                        dictionary.Add(item.Identifier, DateTime.Now.ToString(DateLongFormat, CultureInfo.InvariantCulture));
                    }

                    else if (item.PropertyName == "Date.NowShort")
                    {
                        dictionary.Add(item.Identifier, DateTime.Now.ToString(DateShortFormat, CultureInfo.InvariantCulture));
                    }

                    #endregion

                }
                else
                {
                    #region Extended Case

                    if (_case.CaseExtendedCaseDatas != null)
                    {
                        dictionary.Add(item.Identifier, GetExtendedCaseValue(_case, propertyName, displayName));
                    }

                    #endregion
                }
            }

            return dictionary;

        }


        private string ReplaceWithValue(int id, int caseId, string text)
        {
            Dictionary<string, string> dictionary = GetCaseValueDictionary(id, caseId);

            foreach (var item in dictionary)
            {
                // text = text.Replace("[" + item.Key + "]", item.Value);
                text = text.Replace(item.Key, item.Value);
            }

            return text;
        }

        public CaseDocumentModel GetCaseDocument(Guid caseDocumentGUID, int caseId)
        {
           var _case = _caseService.GetCaseById(caseId);


            var caseDocument = this._caseDocumentRepository.GetCaseDocument(caseDocumentGUID);

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
                            //TODO: Refactor. For performance, at the moment we are fetching values that dont need to be fetched.
                            if (checkCaseDocumentTextConditions(_case, paragraphText.Id))
                            {
                                paragraphText.Text = ReplaceWithValue(caseDocument.Id, caseId, paragraphText.Text);
                            }
                            else
                            {
                                paragraphText.Text = "";
                            }

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
                CaseDocumentGUID = c.CaseDocumentGUID

            }).OrderBy(c => c.SortOrder).ToList();

            return caseDocuments.Where(c => showCaseDocument(customerId, _case, c.Id, user, applicationType) == true).ToList();
        }

        //TODO: REFACTOR, this could be used the "same" way as workflowstep... 
        private bool showCaseDocument(int customerId, Case _case, int caseDocumentId, UserOverview user, ApplicationType applicationType)
        {
            //ALL conditions must be met
            bool showDocument = false;

            //If no conditions are set in (CaseSolutionCondition) for the template, do not show step in list
            var condtions = this.GetCaseDocumentConditions(caseDocumentId);

            if (condtions == null || condtions.Count() == 0)
                return false;

            foreach (var condition in condtions)
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


        //TODO: REFACTOR
        private bool checkCaseDocumentTextConditions(Case _case, int caseDocumentText_Id)
        {
            //If there are more than one conditions, all conditions must be fulfilled

            //IF there are no conditions set, it should be visible
            bool showText = true;
         
            var condtions = this.GetCaseDocumentTextConditions(caseDocumentText_Id);

            //IF there are no conditions set, it should be visible
            if (condtions == null || condtions.Count() == 0)
                return true;

            foreach (var condition in condtions)
            {
                
                var conditionValue = condition.Values.Tidy().ToLower();
                var conditionKey = condition.Property_Name.Tidy();
                var conditionOperator = condition.Operator.Tidy();

                    try
                {

                    var value = "";

                    //GET FROM EXTENDEDCASE
                    if (conditionKey.ToLower().StartsWith("extendedcase_"))
                    {
                        conditionKey = conditionKey.Replace("extendedcase_", "");
                        value = _extendedCaseValueRepository.GetExtendedCaseValue(_case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData_Id, conditionKey).Value;
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
                    //FOR NOW, only one value (NO COMMA SEPARATION)
                    //TODO: Split values
                    if (condition.Operator.ToLower() == "HasValue".ToLower())
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "IsEmpty".ToLower())
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "Equal".ToLower())
                    {
                      
                        if (value.Length > 0 && conditionValue.ToLower() == value.ToLower())
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "NotEqual".ToLower())
                    {
                        if (value.Length > 0 && conditionValue.ToLower() != value.ToLower())
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "LessThan".ToLower())
                    {
                        
                        if (string.IsNullOrEmpty(value))
                            return false;

                        //ASSUME IT IS AN INT
                        int intValue;
                        int.TryParse(value, out intValue);

                        //ASSUME THAT WE ONLY HAVE ONE VALUE, no commaseparted here
                        int intConditionValue;
                        int.TryParse(condition.Values, out intConditionValue);

                        if (intValue < intConditionValue)
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "LessThanOrEqual".ToLower())
                    {

                        if (string.IsNullOrEmpty(value))
                            return false;

                        //ASSUME IT IS AN INT
                        int intValue;
                        int.TryParse(value, out intValue);

                        //ASSUME THAT WE ONLY HAVE ONE VALUE, no commaseparted here
                        int intConditionValue;
                        int.TryParse(condition.Values, out intConditionValue);

                        if (intValue <= intConditionValue)
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "LargerThan".ToLower())
                    {

                        if (string.IsNullOrEmpty(value))
                            return false;

                        //ASSUME IT IS AN INT
                        int intValue;
                        int.TryParse(value, out intValue);

                        //ASSUME THAT WE ONLY HAVE ONE VALUE, no commaseparted here
                        int intConditionValue;
                        int.TryParse(condition.Values, out intConditionValue);

                        if (intValue > intConditionValue)
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator.ToLower() == "LargerThanOrEqual".ToLower())
                    {

                        if (string.IsNullOrEmpty(value))
                            return false;

                        //ASSUME IT IS AN INT
                        int intValue;
                        int.TryParse(value, out intValue);

                        //ASSUME THAT WE ONLY HAVE ONE VALUE, no commaseparted here
                        int intConditionValue;
                        int.TryParse(condition.Values, out intConditionValue);

                        if (intValue >= intConditionValue)
                        {
                            showText = true;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
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

            //If there are more than one conditions, all conditions must be fulfilled
            return showText;
        }


        public Dictionary<string, string> GetCaseDocumentConditions(int caseDocument_Id)
        {
            Dictionary<string, string> caseDocumentConditions = _caseDocumentConditionRepository.GetCaseDocumentConditions(caseDocument_Id).Select(x => new { x.Property_Name, x.Values }).ToDictionary(x => x.Property_Name, x => x.Values);
            return caseDocumentConditions;
        }

        //public Tuple<string, string,string> GetCaseDocumentTextConditions(int caseDocumentText_Id)
        //{
        //    //.Select(x => new Tuple<app_subjects, bool>(x, false)).ToList();
        //    //.Select(x => new Tuple<>(x.Property_Name, x.Values, x.Operator>)

        //    Tuple<string, string, string> caseDocumentTextConditions = (Tuple<string, string, string>)_caseDocumentTextConditionRepository.GetCaseDocumentTextConditions(caseDocumentText_Id).Select(x => Tuple.Create(x.Property_Name,x.Values,x.Operator));
        //    return caseDocumentTextConditions;
        //}


        public IEnumerable<CaseDocumentTextConditionModel> GetCaseDocumentTextConditions(int caseDocumentText_Id)
        {
            var caseDocumentTextConditions = _caseDocumentTextConditionRepository.GetCaseDocumentTextConditions(caseDocumentText_Id);

            return caseDocumentTextConditions;
        }

    }
}
