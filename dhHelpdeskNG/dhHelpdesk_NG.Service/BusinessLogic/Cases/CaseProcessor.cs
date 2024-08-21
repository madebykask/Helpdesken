using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Common.Exceptions;
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
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseFileService _caseFileService;
        private readonly ILogFileService _logFileService;

        public CaseProcessor(ICaseService caseService, 
                             ICaseTypeService caseTypeService,
                             ISettingService settingsService,
                             ICustomerService customerService,
                             IMasterDataService masterDataService,
                             ICaseFileService caseFileService,
                             ILogFileService logFileService
                             )
        {
            _customerService = customerService;
            _settingsService = settingsService;
            _caseService = caseService;
            _caseTypeService = caseTypeService;
            _masterDataService = masterDataService;
            _caseFileService = caseFileService;
            _logFileService = logFileService;
        }

        #region Move Case To External Customer

        public void MoveCaseToExternalCustomer(int caseId, int userId, int newCustomerId)
        {
            //TODO - Move casefiles
            var @case = _caseService.GetCaseById(caseId);
            var oldCustomerId = @case.Customer_Id;

            var caseCustomerId = @case.Customer_Id;
            var newCustomerSettings = _settingsService.GetCustomerSetting(newCustomerId);
            var newCustomerDefaults = _customerService.GetCustomerDefaults(newCustomerId);

            // override customerId 
            @case.Customer_Id = newCustomerId;
            @case.MovedFromCustomer_Id = oldCustomerId;

            //1. set case typeId
            var newCaseTypeId = 0;
            var caseTypeId = @case.CaseType?.Id ?? 0;
            if (caseTypeId > 0)
            {
                newCaseTypeId = TryMatchCaseTypeForCustomer(caseTypeId, caseCustomerId, newCustomerId);
            }

            if (newCaseTypeId == 0)
                newCaseTypeId = newCustomerDefaults?.CaseTypeId ?? 0;

            if (newCaseTypeId <= 0)
            {
                throw new HelpdeskException("Ärendet kan inte flyttas. Matchande ärendetyp eller standard ärendetyp kan inte hittas.");
            }

            @case.CaseType_Id = newCaseTypeId;

            //2. set case default user as an administrator of the case
            @case.Performer_User_Id = newCustomerSettings.DefaultAdministrator;

            //3. Reset all dropdown fields //what selections?
            ResetCaseFields(@case);

            //4. Save case
            IDictionary<string, string> errors;
            var extraInfo = CaseExtraInfo.CreateHelpdesk5();

            if (@case.CaseExtendedCaseDatas != null)
                _caseService.DeleteExCaseWhenCaseMove(caseId);

            //SAVE CASE
            _caseService.SaveCase(@case, null, userId, null, extraInfo, out errors);

            //move case files
            var fromBasePath = _masterDataService.GetFilePath(oldCustomerId);
            var toPath = _masterDataService.GetFilePath(newCustomerId);

            if (!fromBasePath.Equals(toPath, StringComparison.CurrentCultureIgnoreCase))
            {
                _caseFileService.MoveCaseFiles(@case.CaseNumber.ToString(), fromBasePath, toPath);
                _logFileService.MoveLogFiles(@case.Id, fromBasePath, toPath);
            }

            if (errors.Count > 0)
            {
                var msg = BuildErrorsMessage(errors);
                throw new Exception(msg);
            }
        }

        private int TryMatchCaseTypeForCustomer(int caseTypeId, int caseCustomerId, int newCustomerId)
        {
            var newCustomerCaseTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(newCustomerId, true);
            if (newCustomerCaseTypes != null && newCustomerCaseTypes.Any())
            {
                var customerTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(caseCustomerId, true);
                var srcCaseTypePath = BuildCaseTypePath(caseTypeId, customerTypes, null);
                if (!string.IsNullOrEmpty(srcCaseTypePath))
                {
                    //find case type to match full path
                    var res = FindCaseType(newCustomerCaseTypes, srcCaseTypePath, null);
                    if (res != null)
                        return res.Id;
                }
            }

            return 0;
        }

        private void ResetCaseFields(Case curCase)
        {
            /*** 1. Initiator: ***/
            //field: curCase.User_Id /
            //field: curCase.ReportedBy
            //field: curCase.PersonsName
            //field: curCase.PersonsEmail
            //field: curCase.PersonsPhone
            //field: curCase.PersonsCellphone
            curCase.Region_Id = null;
            curCase.Department_Id = null;
            curCase.OU_Id = null;
            curCase.CostCentre = null;
            //field: curCase.Place
            //field: curCase.UserCode


            /*** 2. Regarding (IsAbout) ***/
            var regarding = curCase.IsAbout;
            if (regarding != null)
            {
                //field: regarding.ReportedBy
                //field: regarding.Person_Name
                //field: regarding.Person_Email
                //field: regarding.Person_Phone
                //field: regarding.Person_Cellphone
                regarding.Region_Id = null;
                regarding.Department_Id = null;
                regarding.OU_Id = null;
                regarding.CostCentre = null;
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
            //field: curCase.RegUserName 
            //field: curCase.RegUserId   

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
            curCase.Supplier_Id = null;
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

            /*** 5. Case Management ***/
            curCase.WorkingGroup_Id = null; 
            curCase.CaseResponsibleUser_Id = null; 
            //curCase.Performer_User_Id // is set to default 
            curCase.Priority_Id = null; 
            curCase.Status_Id = null;  
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
        }


        private string BuildCaseTypePath(int caseTypeId, IList<CaseTypeOverview> caseTypes, string parentPath)
        {
            foreach (var caseType in caseTypes)
            {
                var caseTypePath = string.IsNullOrEmpty(parentPath) ? caseType.Name : $"{parentPath}\\{caseType.Name}";
                if (caseType.Id == caseTypeId)
                {
                    return caseTypePath;
                }

                if (caseType.SubCaseTypes != null && caseType.SubCaseTypes.Any())
                {
                    var res = BuildCaseTypePath(caseTypeId, caseType.SubCaseTypes, caseTypePath);
                    if (res != null)
                        return res;
                }
            }

            return null;
        }

        private CaseTypeOverview FindCaseType(IList<CaseTypeOverview> caseTypes, string srcCaseTypePath, string parentPath)
        {
            foreach (var caseType in caseTypes)
            {
                var caseTypePath = string.IsNullOrEmpty(parentPath) ? caseType.Name : $"{parentPath}\\{caseType.Name}";
                if (srcCaseTypePath.Equals(caseTypePath, StringComparison.OrdinalIgnoreCase))
                {
                    return caseType;
                }

                if (caseType.SubCaseTypes != null && caseType.SubCaseTypes.Any())
                {
                    var res = FindCaseType(caseType.SubCaseTypes, srcCaseTypePath, caseTypePath);
                    if (res != null)
                        return res;
                }
            }

            return null;
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

        private int? GetDefaultCaseTypeId()
        {
            //try set default value from case field settings
            //int defaultCaseTypeId = 0;
            //var fieldSettings = _caseFieldSettingService.GetCaseFieldSettings(newCustomerId);
            //var defaultValue = fieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString())?.DefaultValue;
            //
            //if (string.IsNullOrEmpty(defaultValue) && Int32.TryParse(defaultValue, out defaultCaseTypeId))
            //{
            //    var caseType = _caseTypeService.GetCaseType(defaultCaseTypeId);
            //    if (caseType != null)
            //        return caseType.Id;
            //}
            return null;
        }

        #endregion
    }
}

