using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.CaseDocument;
using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.BusinessData.Models.Language.Output;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Grid.Output;
using DH.Helpdesk.Web.Models.Case.ChildCase;
using DH.Helpdesk.Web.Models.Case.Output;
using DH.Helpdesk.Web.Models.CaseLock;
using DH.Helpdesk.Web.Models.Invoice;
using DH.Helpdesk.Web.Models.Shared;

using ParentCaseInfo = DH.Helpdesk.Web.Models.Case.ChildCase.ParentCaseInfo;
using MergedParentInfo = DH.Helpdesk.Web.Models.Case.ChildCase.MergedParentInfo;
using DH.Helpdesk.BusinessData.Models.Case.Input;
using DH.Helpdesk.BusinessData.Models.Case.MergedCase;

namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseHistoryViewModel
    {
        public int? CaseCustomerId { get; set; }
        public int DepartmentFilterFormat { get; set; }
        public TimeZoneInfo UserTimeZone { get; set; }
        public OutputFormatter OutFormatter { get; set; }
        public IList<CaseFieldSetting> caseFieldSettings { get; set; }
        public IList<CaseHistoryOverview> CaseHistories { get; set; }
        public List<CustomMailTemplate> MailTemplates { get; set; }
    }

    public class CaseLogFilesViewModel
    {
        public int CaseId { get; set; }
        public string LogId { get; set; } 
        public bool UseVirtualDirectory { get; set; }
        public int CaseNumber { get; set; }
        public CaseFilesUrlBuilder FilesUrlBuilder { get; set; }
        public List<LogFileModel> Files { get; set; }
        public bool IsExternal { get; set; }
        public bool IsTwoAttachmentsMode { get; set; }
    }

    public class CaseLogInputFilesAttachmentViewModel
    {
        public bool IsExternalNote { get; set; }
        public string FieldStyles { get; set; }
        public bool AllowFileAttach { get; set; }
        public bool IsReadonly { get; set; }
        public string Caption { get; set; }
        public string LogFileNames { get; set; }
        public CaseLogFilesViewModel LogFilesModel { get; set; }
    }

    public class CaseInputViewModel
    {
        public CaseLogInputFilesAttachmentViewModel CreateLogInputFilesAttachmentViewModel(
            bool isExternal, 
            string caption, 
            string fieldStyles, 
            bool allowFileAttach,
            bool isReadonly, 
            bool isTwoAttachmentMode)
        {
            var filesNames = LogFileNames;

            var filesModel = isExternal ? LogFilesModel : LogInternalFilesModel;

            return new CaseLogInputFilesAttachmentViewModel
            {
                IsExternalNote = isExternal,
                Caption = caption,
                FieldStyles = fieldStyles,
                IsReadonly = isReadonly,
                AllowFileAttach = allowFileAttach, 
                LogFileNames = filesNames,

                LogFilesModel = filesModel != null ? new CaseLogFilesViewModel
                {
                    LogId = filesModel?.Id,
                    CaseId = CaseId,
                    CaseNumber = Convert.ToInt32(CaseNumber),
                    UseVirtualDirectory = filesModel.VirtualDirectory,
                    FilesUrlBuilder = CaseFilesUrlBuilder,
                    Files = filesModel.Files,
                    IsExternal = isExternal,
                    IsTwoAttachmentsMode = isTwoAttachmentMode,
                } : new CaseLogFilesViewModel()
            };
        }

        public CaseInputViewModel()
        {

            CaseSolutionSettingModels = CaseSolutionSettingModel.CreateDefaultModel();
            CustomerRegistrationSources = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = string.Empty,
                    Value = string.Empty
                }
            };
            ExternalInvoices = new List<ExternalInvoiceModel>();
            SelectedWorkflowStep = 0;
            CaseLock = new CaseLockModel();
            NumberOfCustomers = 0;
        }

        public CaseLogViewModel CreateCaseLogViewModel()
        {
#pragma warning disable 0618
            return new CaseLogViewModel
            {
                CustomerId = CustomerId,
                CaseNumber = Convert.ToInt32(CaseNumber),
                CaseCustomerId = case_?.Customer_Id,
                UserTimeZone = UserTimeZone,
                CurrentCaseLanguageId = CurrentCaseLanguageId,
                IsCaseReopened = IsCaseReopened,
                CaseFiles = CaseFilesModel,
                HelpdeskEmail = case_?.Customer?.HelpdeskEmail,
                CaseInternalLogAccess = CaseInternalLogAccess,
                Logs = Logs,
                Setting  = Setting,
                CaseFieldSettings = caseFieldSettings,
                CustomerSettings = CustomerSettings,
                CaseSolutionSettingModels = CaseSolutionSettingModels,
                FilesUrlBuilder = CaseFilesUrlBuilder,
                IsTwoAttachmentsMode = EnableTwoAttachments,

                // Invoices fields
                ShowInvoiceFields = ShowInvoiceFields,
                ShowExternalInvoiceFields = ShowExternalInvoiceFields,
                ExternalInvoices = ExternalInvoices,
            };
#pragma warning restore 0618
        }

        public CaseHistoryViewModel CreateHistoryViewModel()
        {
#pragma warning disable 0618
            return new CaseHistoryViewModel()
            {
                CaseCustomerId = case_?.Customer_Id,
                DepartmentFilterFormat = DepartmentFilterFormat,
                UserTimeZone = UserTimeZone,
                OutFormatter = OutFormatter,
                caseFieldSettings = caseFieldSettings,
                CaseHistories = CaseHistories?.OrderByDescending(x => x.Id).ToList() ?? new List<CaseHistoryOverview>(),
                MailTemplates = MailTemplates
            };
#pragma warning restore 0618
        }

        public string CaseKey { get; set; }
        public string LogKey { get; set; }
        public string ParantPath_CaseType { get; set; }
        public string ParantPath_ProductArea { get; set; }
        public string ParantPath_Category { get; set; }
        public string ParantPath_OU { get; set; }
        public int DepartmentFilterFormat { get; set; }
        public int? CountryId { get; set; }
        public int ShowInvoiceFields { get; set; }
        public int ShowExternalInvoiceFields { get; set; }
        public bool TimeRequired { get; set; }
        public CaseLockModel CaseLock { get; set; }
        public int MinWorkingTime { get; set; }        
        public AccessMode EditMode { get; set; } //(-1,0,1)
        public bool Disable_SendMailAboutCaseToNotifier { get; set; }
        public int ProductAreaHasChild { get; set; }
        public int FinishingCauseHasChild { get; set; }

        public int CategoryHasChild { get; set; }
        public int? OrderId { get; set; }
        public int? AccountId { get; set; }
        public int? AccountActivityId { get; set; }
        public string ActiveTab { get; set; }
        public int? SelectedWorkflowStep { get; set; }
        public int CurrentCaseLanguageId { get; set; }
        public int NumberOfCustomers { get; set; }

        [Obsolete("Put all fields that you required into this CaseInputViewModel model")]
        public Domain.Case case_  { get; set; }

        public int CaseId
        {
#pragma warning disable 0618
            get { return case_?.Id ?? 0; }
#pragma warning restore 0618
        }


        public int CustomerId
        {
#pragma warning disable 0618
            get { return case_?.Customer_Id ?? 0; }
#pragma warning restore 0618
        }

        public decimal CaseNumber
        {
#pragma warning disable 0618
            get { return case_?.CaseNumber ?? 0; }
#pragma warning restore 0618
        }

        public IList<string> WhiteFilesList { get; set; }
        public int MaxFileSize { get; set; }

        public IList<CaseHistoryOverview> CaseHistories { get; set; }
        public CaseLog CaseLog { get; set; }
        public SendToDialogModel SendToDialogModel { get; set; }
        public SendToDialogModel FollowersModel { get; set; }

        public CaseMailSetting CaseMailSetting { get; set; }
        public User RegByUser { get; set; }
        public CustomerUser customerUserSetting { get; set; }

        public Setting Setting { get; set; }

        public IList<CaseFieldSetting> caseFieldSettings { get; set; }

        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }
        public IList<CaseSolutionSettingModel> CaseSolutionSettingModels { get; set; }

        public IEnumerable<CaseSectionModel> CaseSectionModels { get; set; }

        public IList<CaseTypeOverview> caseTypes { get; set; }
        public IList<StandardText> standardTexts { get; set; }
        public IList<CategoryOverview> categories { get; set; }
        public IList<ChangeOverview> changes { get; set; }
        public IList<Country> countries { get; set; }
        public IList<Currency> currencies { get; set; }
        public IList<Department> departments { get; set; }
        public IList<FinishingCauseOverview> finishingCauses { get; set; }
        public IList<Impact> impacts { get; set; }
        public IList<ProblemOverview> problems { get; set; }
        public IList<ProductAreaOverview> productAreas { get; set; }
        public IList<Priority> priorities { get; set; }
        public IList<ProjectOverview> projects { get; set; }
        public IList<OU> ous { get; set; }  //unit
        public IList<OU> isaboutous { get; set; }  //Is about ous
        public IList<Region> regions { get; set; }
        public IList<Status> statuses { get; set; }
        public IList<StateSecondary> stateSecondaries { get; set; }
        public IList<Supplier> suppliers { get; set; }
        public IList<Helpdesk.Domain.System> systems { get; set; }
        public IList<Urgency> urgencies { get; set; }        
        //public IEnumerable<CausingPartOverview> causingParts { get; set; }        
        public List<SelectListItem> causingParts { get; set; }

        public bool UserHasInvoicePermission { get; set; }

        public bool UserHasInventoryViewPermission { get; set; }

        /// <summary>
        /// user id for "Administrator" field
        /// </summary>
        public int Performer_Id { get; set; }

        /// <summary>
        /// Available  "Adminstrators" for the case
        /// </summary>
        public SelectList Performers { get; set; }

        /// <summary>
        /// Available  "Performs and working groups to search" for the case
        /// </summary>
        public List<CasePerformersSearch> PerformersToSearch { get; set;}

        /// <summary>
        /// user id for "Responsible" field
        /// </summary>
        public int ResponsibleUser_Id { get; set; }

        /// <summary>
        /// Available users for "Responsible" case field
        /// </summary>
        public SelectList ResponsibleUsersAvailable { get; set; }

        public IList<WorkingGroupEntity> workingGroups { get; set; }

        public IList<LogOverview> Logs { get; set; }

        /// <summary>
        /// Selected case source
        /// </summary>
        public int CustomerRegistrationSourceId { get; set; }

        /// <summary>
        /// Selected case source name
        /// </summary>
        public string SelectedCustomerRegistrationSource { get; set; }

        /// <summary>
        /// List of available case sources
        /// </summary>
        public List<SelectListItem> CustomerRegistrationSources { get; set; }

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        public IEnumerable<LanguageOverview> Languages { get; set; }
  
        public CaseHistory EmptyCaseHistory { get; set; }

        public List<CustomMailTemplate> MailTemplates { get; set; }
        
        public FilesModel LogFilesModel { get; set; }
        
        public FilesModel LogInternalFilesModel { get; set; }

        public CaseFilesModel CaseFilesModel { get; set; }

        public string CaseFileNames { get; set; }

        public string LogFileNames { get; set; }

        public string SavedFiles { get; set; }
        
        /// <summary>
        /// Gets or sets the case owner default working group.
        /// </summary>
        public WorkingGroupEntity CaseOwnerDefaultWorkingGroup { get; set; }

        public bool InformNotifierBehavior { get; set; }

        public bool InformPerformerBehavior { get; set; }

        public bool UpdateNotifierInformation { get; set; }

        public bool AddFollowersBtn { get; set; }

        public string FinishingCause { get; set; }
     
        public CaseInvoiceModel InvoiceModel { get; set; }

        public CustomerSettings CustomerSettings { get; set; }

        public NewCaseParams  NewModeParams { get; set; }

        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }

        public DynamicCase DynamicCase { get; set; }

        public int? templateistrue { get; set; }

        public string CaseTemplateName { get; set; }
        public int? CaseTemplateSplitToCaseSolutionID { get; set; }

        public string BackUrl { get; set; }

        public bool CanGetRelatedCases { get; set; }

        public IList<ExtendedCaseFormForCaseModel> ExtendedCases { get; set; }

        public IList<ExtendedCaseFormForCaseModel> ComputerUserCategoryExtendedCases { get; set; }

        public IList<CaseDocumentModel> CaseDocuments { get; set; }

        #region Date field from case_. Converted to user time zone

        public DateTime RegTime { get; set; }

        public DateTime ChangeTime { get; set; }

        #endregion

        public ChildCaseViewModel ChildCaseViewModel { get; set; }
        
        public int ClosedChildCasesCount { get; set; }

        public ParentCaseInfo ParentCaseInfo { get; set; }
        public MergedParentInfo MergedParentInfo { get; set; }
        public ReportModel CasePrintView { get; set; }

        public int? MovedFromCustomerId { get; set; }

        public bool IsReturnToCase { get; set; }

        public bool IsCaseReopened { get; set; }

        public bool IsItChildCase()
        {
            return this.ParentCaseInfo != null && ParentCaseInfo.ParentId != 0;
        }
        public bool IsItMergedChild()
        {
            return this.MergedParentInfo != null && MergedParentInfo.ParentId != 0;
        }
        public bool IsItMergedParent()
        {
            return this.ChildCaseViewModel != null && this.ChildCaseViewModel.MergedChildList != null && this.ChildCaseViewModel.MergedChildList.Count > 0;
        }
        public bool IsItParentCase()
        {
            return this.ChildCaseViewModel != null && this.ChildCaseViewModel.ChildCaseList != null && this.ChildCaseViewModel.ChildCaseList.Count > 0;
        }

        public string CaseRelationType()
        {
            if (this.IsItChildCase())
            {
                return "Child";
            }

            if (this.IsItParentCase())
            {
                return "Parent";
            }
            if (this.IsItMergedChild())
            {
                return "Merged";
            }
            if (this.IsItMergedParent())
            {
                return "MergedParent";
            }
            return "";
        }

        public bool IsAnyNotClosedChild(bool containsIndependents = false)
        {
            if (this.ChildCaseViewModel == null)
            {
                return false;
            }

            var childList = this.ChildCaseViewModel.ChildCaseList;
            if (childList == null || childList.Count == 0)
            {
                return false;
            }

            return childList.Any(it => it.ClosingDate == null && (containsIndependents? !it.Indepandent : true));
        }
        public bool IsAnyNotClosedNonIndependentChild()
        {
            // Check if the ChildCaseViewModel is null.
            if (this.ChildCaseViewModel == null)
            {
                return false;
            }

            var childList = this.ChildCaseViewModel.ChildCaseList;
            // Check if the child list is null or empty.
            if (childList == null || childList.Count == 0)
            {
                return false;
            }

            // Iterate through each child case in the list.
            foreach (var childCase in childList)
            {
                // Check if the child case is not independent and does not have a closing date.
                if (!childCase.Indepandent && childCase.ClosingDate == null)
                {
                    return true; // Return true if such a child case is found.
                }
            }

            // If no matching child case is found, return false.
            return false;
        }


        public string ExtendedSectionsToJS()
        {
            var sections = ExtendedCaseSections;
            if (sections == null || !sections.Any())
                return "null";

            var sb = new StringBuilder();
            sb.Append("{" + Environment.NewLine);

            foreach (var section in sections)
            {
                var caseSectionType = section.Key;
                var model = section.Value;
                var readOnly = (section.Key == CaseSectionType.Initiator && InitiatorComputerUserCategory != null && InitiatorComputerUserCategory.IsReadOnly) ||
                               (section.Key == CaseSectionType.Regarding && RegardingComputerUserCategory != null && RegardingComputerUserCategory.IsReadOnly);

                var str = $@"{caseSectionType}: {{ 
                                formId: {model.Id}, 
                                guid: '{model.ExtendedCaseGuid}', 
                                languageId: '{model.LanguageId}', 
                                path: '{model.Path}', 
                                iframeId: '#extendedSection-iframe-{caseSectionType}',
                                container:  '#extendedSection-{caseSectionType}',
                                readOnly: {readOnly.ToString().ToLower()}
                            }},";
                sb.Append(str);
            }

            sb.Remove(sb.Length - 1, 1); // Remove last ","

            sb.Append(Environment.NewLine + "}");

            var result = sb.ToString();
            return result;
        }

        public OutputFormatter OutFormatter { get; set; }

        public CaseFilesUrlBuilder CaseFilesUrlBuilder { get; set; }

        public List<CaseTemplateButton> CaseTemplateButtons { get; set; }

        public bool IsFollowUp { get; set; }

        public bool CaseUnlockAccess { get; set; }
        public bool CaseInternalLogAccess { get; set; }

        public List<ExternalInvoiceModel> ExternalInvoices { get; set; }

        public string FollowerUsers { get; set; }

        public JsonCaseIndexViewModel ConnectToParentModel { get; set; }
        public bool newLog { get; set; }
        public bool editLog { get; set; }
        public IList<WorkflowStepModel> WorkflowSteps { get; set; }
        public bool IsRelatedCase { get; set; }

        public int LanguageId { get; set; }

        public bool ContainsExtendedCase { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public bool IndependentChild { get; internal set; }
        public Domain.CaseSolution CurrentCaseSolution { get; internal set; }
        public string CurrentUserName { get; set; }
        public int CurrentUserRole { get; set; }
        
        public Dictionary<string, string> StatusBar { get; internal set; }
        public bool HasExtendedComputerUsers { get; internal set; }
        public IList<ComputerUserCategoryOverview> ComputerUserCategories { get; internal set; }
        public string EmptyComputerCategoryName { get; set; }

        public ComputerUserCategory InitiatorComputerUserCategory { get; internal set; }
        public bool InitiatorReadOnly { get; set; }
        
        public IDictionary<CaseSectionType, ExtendedCaseFormForCaseModel> ExtendedCaseSections { get; internal set; }
        public ComputerUserCategory RegardingComputerUserCategory { get; internal set; }
        public bool RegardingReadOnly { get; internal set; }

        public int? InitiatorCategoryId
        {
            get { return InitiatorComputerUserCategory?.ID; }
        }

        public int? RegardingCategoryId
        {
            get { return RegardingComputerUserCategory?.ID; }
        }

        public bool IsUserAdmin
        {
            get { return CurrentUserRole > (int)BusinessData.Enums.Admin.Users.UserGroup.User; }
        }

        public TimeZoneInfo UserTimeZone { get; set; }
        public bool EnableTwoAttachments { get; internal set; }
		public bool HasFileUploadWhiteList { get; internal set; }
		public List<string> FileUploadWhiteList { get; internal set; }
	}

    public class CaseIndexViewModel
    {
        public int CustomerId { get; set; }
        public CaseSearchFilterData caseSearchFilterData { get; set; }
        public CaseSearchResultModel caseSearchResult { get; set; }
        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }
        public CaseSettingModel CaseSetting { get; set; }
    }

    public class CaseTemplateButton
    {
        public int CaseTemplateId { get; set; }
        public string CaseTemplateName { get; set; }
        public int ButtonNumber { get; set; }        
    }

    public class PageSettingsModel
    {
        /// <summary>
        /// Holds values for filter form in case overview page
        /// </summary>
        public JsonCaseSearchFilterData searchFilter { get; set; }

        public JsonGridSettingsModel gridSettings { get; set; }

        public List<MyFavoriteFilterJSModel> userFilterFavorites { get; set; }

        public Dictionary<string, string> messages { get; set; }

        public int refreshContent { get; set; }

        public string HelperRegTime { get; set; }

        public string HelperCaption { get; set; }

    }

    public class JsonCaseIndexViewModel
    {       
        public CaseSearchFilterData CaseSearchFilterData { get; set; }

        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }

        public CaseSettingModel CaseSetting { get; set; }

        public PageSettingsModel PageSettings { get; set; }

        public CaseRemainingTimeViewModel RemainingTime { get; set; }
        public CaseInputViewModel CaseInputViewModel { get; set; }
    }

    public class AdvancedSearchIndexViewModel
    {
        public bool DoSearchAtBegining { get; set; }

        public bool IsExtSearch { get; set; }

        public CaseSearchFilterData CaseSearchFilterData { get; set; }

        public AdvancedSearchSpecificFilterData SpecificSearchFilterData { get; set; }

        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }

        public CaseSettingModel CaseSetting { get; set; }

        public JsonGridSettingsModel GridSettings { get; set; }

        public CaseRemainingTimeViewModel RemainingTime { get; set; }

        public List<ItemOverview> UserCustomers { get; set; }

        public List<ItemOverview> ExtendedCustomers { get; set; }

        public List<ItemOverview> AllCustomers
        {
            get
            {
                var allCustomers = new List<ItemOverview>();

                if (UserCustomers.Any())
                    allCustomers.AddRange(UserCustomers);

                if (ExtendedCustomers.Any())
                    allCustomers.AddRange(ExtendedCustomers);

                return allCustomers.OrderBy(x => x.Name).ToList();
            }
        }
    }

    public class CaseSearchResultModel
    {
        public CaseSearchResultModel()
        {
            caseSettings = new List<CaseSettings>();
        }

        public CaseColumnsSettingsModel GridSettings { get; set; }
        
        public IList<CaseSettings> caseSettings { get; set; }

        public IList<CaseSearchResult> cases { get; set; }

        public string BackUrl { get; set; }

        public bool ShowRemainingTime { get; set; }

        public CaseRemainingTimeViewModel RemainingTime { get; set; }
    }

    public class NewCaseParams
    {
        public int customerId { get; set; }
        public int? templateId { get; set; }
        public int? copyFromCaseId { get; set; }
        public int? caseLanguageId { get; set; }
    }    

}