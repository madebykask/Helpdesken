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

        public CaseDocumentService(
            ICaseDocumentRepository caseDocumentRepository,
            ICaseDocumentConditionRepository caseDocumentConditionRepository,
            ICacheProvider cache,
            IExtendedCaseValueRepository extendedCaseValueRepository,
             ICaseService caseService,
              ICaseDocumentTextConditionRepository caseDocumentTextConditionRepository
            )
        {
            this._caseDocumentRepository = caseDocumentRepository;
            this._caseDocumentConditionRepository = caseDocumentConditionRepository;
            this._cache = cache;
            this._extendedCaseValueRepository = extendedCaseValueRepository;
            this._caseService = caseService;
            this._caseDocumentTextConditionRepository = caseDocumentTextConditionRepository;
        }

        private string GetExtendedCaseValue(Case _case, string fieldId)
        {
            int pos = fieldId.LastIndexOf(".") + 1;

            string fieldIdShort =  fieldId.Substring(pos, fieldId.Length - pos);



            
            try
            {
                // return  _extendedCaseValueRepository.GetExtendedCaseValue(_case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData_Id, fieldId).Value;

                //Check if SecondaryValue exist
                var value = _case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData.ExtendedCaseValues.Where(x => x.FieldId.ToLower() == fieldId.ToLower()).First().SecondaryValue;

                //REFACTOR
                if (string.IsNullOrEmpty(value))
                {
                    //Check for value
                    value = _case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData.ExtendedCaseValues.Where(x => x.FieldId.ToLower() == fieldId.ToLower()).First().Value;

                    if (string.IsNullOrEmpty(value))
                    {
                       return "[" + fieldIdShort + "]";
                    }
                }

                return value;
            }
            catch (Exception ex)
            {
                return "[" + fieldIdShort + "]"; 
            }
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


            string caseReportedBy = "Case.ReportedBy";
            dictionary.Add(caseReportedBy, GetCaseValue(_case, caseReportedBy));


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

            if (_case.CaseExtendedCaseDatas != null)
            {
                string fieldId = "tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.AddressLine1";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.AddressLine2";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.State";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.PostalCode";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.instances[0].controls.AddressLine3";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));
                
                fieldId = "tabs.OrganisationalAssignment.sections.STD_S_OrganisationHiring.instances[0].controls.ReportsToLineManagerPositionTitle";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.OrganisationalAssignment.sections.STD_S_OrganisationHiring.instances[0].controls.ReportsToLineManager";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.OrganisationalAssignment.sections.STD_S_JobHiring.instances[0].controls.PositionTitleLocalJobName";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.ServiceRequestDetails.sections.STD_S_ProcessingDetailsHiring.instances[0].controls.ContractStartDate";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditionsHiring.instances[0].controls.ContractedHours";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

                fieldId = "tabs.OrganisationalAssignment.sections.STD_S_EmploymentConditionsHiring.instances[0].controls.ContractEndDate";
                dictionary.Add(fieldId, GetExtendedCaseValue(_case, fieldId));

            }

            /* KOLLA KONTRAKTET */




            ////[CoworkerGlobalViewID]
            //dictionary.Add("[CoworkerGlobalViewID]", "[CoworkerGlobalViewID]");



            ////[tabs.OrganisationalAssignment.sections.STD_S_JobHiring.instances[0].controls.PositionTitleLocalJobName]
            //dictionary.Add("[tabs.OrganisationalAssignment.sections.STD_S_JobHiring.instances[0].controls.PositionTitleLocalJobName]", "[tabs.OrganisationalAssignment.sections.STD_S_JobHiring.instances[0].controls.PositionTitleLocalJobName]");

            ////[ReportsToLineManager.PositionTitle]
            //dictionary.Add("[ReportsToLineManager.PositionTitle]", "[ReportsToLineManager.PositionTitle]");

            ////[ReportsToLineManager]
            //dictionary.Add("[ReportsToLineManager]", "[ReportsToLineManager]");


            ////tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.controls.AddressLine1
            ////m.tabs.ServiceRequestDetails.sections.STD_S_PermanentAddressHiring.controls.AddressLine2

            ////[AddressLine1]
            //dictionary.Add("[AddressLine1]", "[AddressLine1]");
            ////[AddressLine2]
            //dictionary.Add("[AddressLine2]", "[AddressLine2]");
            ////[State]
            //dictionary.Add("[State]", "[State]");
            ////[PostalCode]
            //dictionary.Add("[PostalCode]", "[PostalCode]");
            ////[AddressLine3]
            //dictionary.Add("[AddressLine3]", "[AddressLine3]");
            ////[ContractStartDate]
            //dictionary.Add("[ContractStartDate]", "[ContractStartDate]");
            ////[BasicPayAmount]
            //dictionary.Add("[BasicPayAmount]", "[BasicPayAmount]");
            ////[ContractedHours]
            //dictionary.Add("[ContractedHours]", "[ContractedHours]");
            ////[NextSalaryReviewYear]
            //dictionary.Add("[NextSalaryReviewYear]", "[NextSalaryReviewYear]");

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
