using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Services.BusinessLogic.Cases
{
    public interface ICaseProcessor
    {
        void MoveCaseToExternalCustomer(int caseId, int userId, int newCustomerId);
    }

    public class CaseProcessor : ICaseProcessor
    {
        private readonly ICaseService _caseService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ISettingService _settingsService;
        private readonly ICustomerService _customerService;

        public CaseProcessor(ICaseService caseService, 
                             ICaseTypeService caseTypeService,
                             ISettingService settingsService,
                             ICustomerService customerService)
        {
            _customerService = customerService;
            _settingsService = settingsService;
            _caseService = caseService;
            _caseTypeService = caseTypeService;
        }

        public void MoveCaseToExternalCustomer(int caseId, int userId, int newCustomerId)
        {
            var @case = _caseService.GetCaseById(caseId);

            var newCustomerSettings = _settingsService.GetCustomerSetting(newCustomerId);
            //var newCustomerFieldsSettings = _caseFieldSettingService.GetCaseFieldSettings(newCustomerId);
            
            var newCustomerDefaults = _customerService.GetCustomerDefaults(newCustomerId);

            // override customerId 
            @case.Customer_Id = newCustomerId;

            //1. set case typeId
            @case.CaseType_Id = GetCaseTypeForCustomer(@case.CaseType_Id, newCustomerId, newCustomerDefaults.CaseTypeId); 

            //2. set case default user as an administrator of the case
            @case.Performer_User_Id = newCustomerSettings.DefaultAdministrator.GetValueOrDefault(0);

            //3. Reset all dropdown fields //what selections?
            ResetCaseFields(newCustomerId, @case);

            //5. log notes and attachments - check if new customer_Id should be set!!!

            //6. Case files - check if files shoud be saved with different customerId: this._caseFileService.GetCaseFiles(caseId, canDelete)

            //7. Save case

            IDictionary<string, string> errors;
            var extraInfo = CaseExtraInfo.CreateHelpdesk5();
            CaseLog caseLog = null;

            //SAVE CASE
            _caseService.SaveCase(@case, caseLog, userId, null, extraInfo, out errors);

            if (errors.Count > 0)
            {
                var msg = BuildErrorsMessage(errors);
                throw new Exception(msg);
            }
        }
        
        private int GetCaseTypeForCustomer(int caseTypeId, int newCustomerId, int defaultCaseCaseTypeId)
        {
            if (caseTypeId > 0)
            {
                var customerCaseTypeIds = _caseTypeService.GetCaseTypeIds(newCustomerId);
                if (customerCaseTypeIds.Contains(caseTypeId))
                {
                    return caseTypeId;
                }
            }

            //try set default value from case field settings
            //int defaultCaseTypeId = 0;
            //var defaultValue = fieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString())?.DefaultValue;
            //
            //if (string.IsNullOrEmpty(defaultValue) && Int32.TryParse(defaultValue, out defaultCaseTypeId))
            //{
            //    var caseType = _caseTypeService.GetCaseType(defaultCaseTypeId);
            //    if (caseType != null)
            //        return caseType.Id;
            //}

            return defaultCaseCaseTypeId > 0 ? defaultCaseCaseTypeId : 0;
        }
 
        private void ResetCaseFields(int newCustomerId, Case curCase)
        {
            curCase.Customer_Id = newCustomerId;

            //todo: check Followers?  
            //m.IsFollowUp = _caseFollowUpService.IsCaseFollowUp(SessionFacade.CurrentUser.Id, caseId);

            //todo: WorkingGroup_Id = this._userRepository.GetUserDefaultWorkingGroupId(userId, customerId),

            /*** 1. Initiator: ***/
            //curCase.User_Id //todo: check if shall keep
            // todo: check Model.InitiatorComputerUserCategory.ID
            //field: curCase.ReportedBy
            //field: curCase.PersonsName
            //field: curCase.PersonsEmail
            //field: curCase.PersonsPhone
            //field: curCase.PersonsCellphone
            curCase.Region_Id = null;
            curCase.Department_Id = null;
            curCase.OU_Id = null;
            //field: curCase.CostCentre = null; //todo: check if empty  since its a field
            //field: curCase.Place
            //field: curCase.UserCode


            /*** 2. Regarding (IsAbout) ***/
            //Model.RegardingComputerUserCategory todo: check
            var regarding = curCase.IsAbout;
            if (regarding != null)
            {
                //todo: shall we reset IsAbout section?
                // regarding.ReportedBy //todo: check if user should be kept
                //field: regarding.ReportedBy
                //field: regarding.Person_Name
                //field: regarding.Person_Email
                //field: regarding.Person_Phone
                //field: regarding.Person_Cellphone
                regarding.Region_Id = null;  //todo: check if default values should be set as during case init ?
                regarding.Department_Id = null;
                regarding.OU_Id = null;
                //field: regarding.CostCentre = null; //todo: check if empty  since its a field
                //field: regarding.Place
                //field: regarding.UserCode
            }

            /*** 3. Computer information ***/
            //field: curCase.InventoryNumber
            //field: curCase.InventoryType
            //field: curCase.InventoryLocation

            /*** 4. Case information ***/
            curCase.RegistrationSourceCustomer_Id = null;
            //field: curCase.CaseNumber
            //field: curCase.RegTime
            //field: curCase.ChangeTime

            //registered by fields
            //field: RegBy : this._userService.GetUser(m.case_.User_Id.Value)
            //else:
            //field: curCase.RegUserName //todo: check if empty?
            //field: curCase.RegUserId   //todo: check if empty?

            // reportedBy: GetComputerUserByUserID - keep
            //m.RegByUser 
            //m.case_.RegistrationSourceCustomer_Id.HasValue)
            //m.CustomerRegistrationSourceId = m.case_.RegistrationSourceCustomer_Id.Value;
            //m.SelectedCustomerRegistrationSource = m.case_.RegistrationSourceCustomer.SourceName;

            //caseType: is handled separately
            curCase.ProductArea_Id = null;
            curCase.System_Id = null;
            curCase.Urgency_Id = null;
            curCase.Impact_Id = null;
            curCase.Category_Id = null;
            curCase.Supplier_Id = null; //todo: set to default?
            //field: curCase.InvoiceNumber 
            //field: curCase.ReferenceNumber
            //field: curCase.Caption
            //field: curCase.Description
            //field: curCase.Miscellaneous
            //field: curCase.ContactBeforeAction
            //field: curCase.SMS
            //field: curCase.AgreedDate
            //field: curCase.Available
            //field: curCase.Cost
            //field: curCase.OtherCost

            //todo:CaseFiles - check if there's customer specific information?
            // Partial("_CaseFiles", Model.CaseFilesModel, new ViewDataDictionary { { "CaseInactive", Model.case_.FinishingDate.HasValue } })


            /*** 5. Case Management ***/
            curCase.WorkingGroup_Id = null; //todo: check if default working group should be set
            curCase.CaseResponsibleUser_Id = null; //todo: check if we need to reset it ?
            //curCase.Performer_User_Id // is set to default 
            curCase.Priority_Id = null; //todo: set to default?
            curCase.Status_Id = null;   //todo: set to default?
            curCase.StateSecondary_Id = null;
            curCase.Project_Id = null;
            curCase.Problem_Id = null;
            //field: curCase.PlanDate
            //field: curCase.WatchDate 
            //field: curCase.Verified
            //field: curCase.VerifiedDescription
            //field: curCase.SolutionRate
            curCase.CausingPartId = null;

            /*** 6. Communication ****/
            //field: curCase.FinishingDescription
            //field: curCase.FinishingDate

            //todo: how to handle?
            // log notes 
            // finishing cause - last log note finishingCause! //todo: check if we need to reset or keep? this._finishingCauseService.GetFinishingCauseInfos(customerId);
            // case files
        }

        private string BuildErrorsMessage(IDictionary<string, string> errors)
        {
            var strBld = new StringBuilder();
            foreach (var errorKv in errors)
            {
                strBld.AppendFormat("{0}:{1}", errorKv.Key, errorKv.Value).AppendLine();
            }
            return strBld.ToString();
        }
    }

    //Todo: check if fields should be checked 
    // m.caseTypes = this._caseTypeService.GetCaseTypesOverviewWithChildren(customerId, takeOnlyActive).OrderBy(c => Translation.GetMasterDataTranslation(c.Name)).ToList();
    // m.categories = this._categoryService.GetParentCategoriesWithChildren(customerId, true);
    // m.priorities = this._priorityService.GetPriorities(customerId);
    // m.impacts = this._impactService.GetImpacts(customerId);
    // m.productAreas = this._productAreaService.GetTopProductAreasForUserOnCase(customerId, m.case_.ProductArea_Id, SessionFacade.CurrentUser).OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();
    // m.regions = this._regionService.GetRegions(customerId);
    // m.statuses = this._statusService.GetStatuses(customerId);
    // m.stateSecondaries = this._stateSecondaryService.GetStateSecondaries(customerId);
    // m.suppliers = this._supplierService.GetSuppliers(customerId);
    // m.countries = this._countryService.GetCountries(customerId);
    // m.systems = this._systemService.GetSystems(customerId);
    // m.causingParts = GetCausingPartsModel(customerId, m.case_.CausingPartId);
    // m.urgencies = this._urgencyService.GetUrgencies(customerId);
    // m.workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive); //default: m.workingGroups.Where(it => it.IsDefault == 1).FirstOrDefault();
    // m.projects = this._projectService.GetCustomerProjects(customerId);
    // m.finishingCauses = this._finishingCauseService.GetFinishingCausesWithChilds(customerId);
    // m.problems = this._problemService.GetCustomerProblems(customerId, false);
    // m.currencies = this._currencyService.GetCurrencies();
    // m.changes = this._changeService.GetChanges(customerId);
    // m.departments = _departmentService.GetDepartments(customerId).Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
    // m.ous = this._organizationService.GetOUs(m.case_.Department_Id).ToList();
    // m.isaboutous = this._organizationService.GetOUs(m.case_.IsAbout.Department_Id).ToList();
    // m.standardTexts = this._standardTextService.GetStandardTexts(customerId);
    // m.ComputerUserCategories = _computerService.GetComputerUserCategoriesByCustomerID(customerId);


    //check if finishing cause should be checked
    //var finishingCauses = this._finishingCauseService.GetFinishingCauseInfos(customerId);
    //var lastLog = m.Logs.FirstOrDefault(); //todo: check if its correct - order
    //m.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCauses.ToArray(), lastLog.FinishingType);


    //todo: check if followers should be removed ?
    // m.FollowerUsers = _caseExtraFollowersService.GetCaseExtraFollowers(caseId);

    // default performer is set from CaseType:
    // m.case_.Performer_User_Id = c.User_Id.Value; check case type!


    // Logs:  check if logs shoud be update with new customerId
    // m.Logs = this._logService.GetCaseLogOverviews(caseId);

    //attached files - shall we update to new customerID?
    // m.CaseAttachedExFiles = _caseFileService.GetCaseFiles(caseId, canDelete)

    //supplier Id
    /*
     if (m.case_.Supplier_Id > 0 && m.suppliers != null)
     {
         var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
         m.CountryId = sup?.Country_Id.GetValueOrDefault();
     }     
     */

    //IsAbout_OU:
    /*
       if (m.case_.IsAbout != null)
            m.isaboutous = this._organizationService.GetOUs(m.case_.IsAbout.Department_Id).ToList();
        else
            m.isaboutous = null;
     */

    //Working groups1:
    //m.case_.WorkingGroup_Id = 
    //m.workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive);
    /*
       var defWorkingGroup = m.workingGroups.Where(it => it.IsDefault == 1).FirstOrDefault();
                if (defWorkingGroup != null)
                {
                    m.case_.WorkingGroup_Id = defWorkingGroup.Id;
                }
    */

    //Working groups2:
    /*
     var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(m.case_.User_Id.Value, m.case_.Customer_Id);
     if (userDefaultWorkingGroupId.HasValue)
     {
        m.case_.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
     
     */

    //Working Group3:
    // m.CaseOwnerDefaultWorkingGroup = this._workingGroupService.GetWorkingGroup(m.case_.DefaultOwnerWG_Id.Value);


    //Registration source?:
    /*        
        CustomerRegistrationSourceId = m.case_.RegistrationSourceCustomer_Id.Value;
        m.SelectedCustomerRegistrationSource = m.case_.RegistrationSourceCustomer.SourceName;
        
        // set default:
        var customerSources = this._registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId).ToArray();
        var defaultSource = customerSources.Where(it => it.SystemCode == (int)CaseRegistrationSource.Administrator).FirstOrDefault();
        m.CustomerRegistrationSourceId = defaultSource.Id;
        m.SelectedCustomerRegistrationSource = defaultSource.SourceName;
     */

    //todo: check child/Parent
    //m.ParentCaseInfo = this._caseService.GetParentInfo(caseId).MapBusinessToWebModel(outputFormatter);
    // var childCases = this._caseService.GetChildCasesFor(caseId);
    //m.ChildCaseViewModel = new ChildCaseViewModel

    //TODO: new customer may not have CaseSolutions of the case?
    //m.CaseTemplateButtons = _caseSolutionService.GetCustomerCaseSolutionsOverview(customerId, userId);

    //TODO: check extended case data!
}

