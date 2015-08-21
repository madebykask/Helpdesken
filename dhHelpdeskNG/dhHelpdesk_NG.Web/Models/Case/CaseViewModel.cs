namespace DH.Helpdesk.Web.Models.Case
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.Models.Logs.Output;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Web.Infrastructure.Grid.Output;
    using DH.Helpdesk.Web.Models.Case.Output;
    using DH.Helpdesk.Web.Models.Invoice;
    using DH.Helpdesk.Web.Models.Shared;
    using DH.Helpdesk.Web.Models.CaseLock;
    
    public class CaseInputViewModel
    {
        public CaseInputViewModel()
        {
            this.CaseSolutionSettingModels = CaseSolutionSettingModel.CreateDefaultModel();
            this.CustomerRegistrationSources = new List<SelectListItem>()
                                                   {
                                                       new SelectListItem()
                                                           {
                                                               Text = string.Empty,
                                                               Value = string.Empty
                                                           }
                                                   };
        }

        public string CaseKey { get; set; }
        public string LogKey { get; set; }
        public string ParantPath_CaseType { get; set; }
        public string ParantPath_ProductArea { get; set; }
        public string ParantPath_OU { get; set; }
        public int DepartmentFilterFormat { get; set; }
        public int? CountryId { get; set; }
        public int ShowInvoiceFields { get; set; }                
        public CaseLockModel CaseLock { get; set; }
        public int MinWorkingTime { get; set; }        
        public Infrastructure.Enums.AccessMode EditMode { get; set; } //(-1,0,1)
        public bool Disable_SendMailAboutCaseToNotifier { get; set; }
        public int ProductAreaHasChild { get; set; }

        [Obsolete("Put all fields that you required into this CaseInputViewModel model")]
        public Case case_  { get; set; }

        public CaseLog CaseLog { get; set; }

        public SendToDialogModel SendToDialogModel { get; set; }
        public CaseMailSetting CaseMailSetting { get; set; }
        public User RegByUser { get; set; }
        public CustomerUser customerUserSetting { get; set; }

        public Setting Setting { get; set; }

        public IList<CaseFieldSetting> caseFieldSettings { get; set; }

        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }
        public IList<CaseSolutionSettingModel> CaseSolutionSettingModels { get; set; }

        public IList<CaseType> caseTypes { get; set; }
        public IList<StandardText> standardTexts { get; set; }
        public IList<Category> categories { get; set; }
        public IList<ChangeEntity> changes { get; set; }
        public IList<Country> countries { get; set; }
        public IList<Currency> currencies { get; set; }
        public IList<Department> departments { get; set; }
        public IList<FinishingCause> finishingCauses { get; set; }
        public IList<Impact> impacts { get; set; }
        public IList<ProblemOverview> problems { get; set; }
        public IList<ProductArea> productAreas { get; set; }
        public IList<Priority> priorities { get; set; }
        public IList<ProjectOverview> projects { get; set; }
        public IList<OU> ous { get; set; }  //unit
        public IList<Region> regions { get; set; }
        public IList<Status> statuses { get; set; }
        public IList<StateSecondary> stateSecondaries { get; set; }
        public IList<Supplier> suppliers { get; set; }
        public IList<Helpdesk.Domain.System> systems { get; set; }
        public IList<Urgency> urgencies { get; set; }
        
        /// <summary>
        /// user id for "Administrator" field
        /// </summary>
        public int Performer_Id { get; set; }

        /// <summary>
        /// Available  "Adminstrators" for the case
        /// </summary>
        public SelectList Performers { get; set; }

        /// <summary>
        /// user id for "Responsible" field
        /// </summary>
        public int ResponsibleUser_Id { get; set; }

        /// <summary>
        /// Available users for "Responsible" case field
        /// </summary>
        public SelectList ResponsibleUsersAvailable { get; set; }

        public IList<WorkingGroupEntity> workingGroups { get; set; }
        public IEnumerable<LogOverview> Logs { get; set; }

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
        
        public FilesModel LogFilesModel { get; set; }

        public CaseFilesModel CaseFilesModel { get; set; }

        public string CaseFileNames { get; set; }

        public string LogFileNames { get; set; }

        /// <summary>
        /// Gets or sets the case owner default working group.
        /// </summary>
        public WorkingGroupEntity CaseOwnerDefaultWorkingGroup { get; set; }

        public bool InformNotifierBehavior { get; set; }

        public bool UpdateNotifierInformation { get; set; }

        public string FinishingCause { get; set; }

        public CaseInvoiceArticlesModel InvoiceArticles { get; set; }

        public string CaseInvoiceArticles { get; set; }

        public CustomerSettings CustomerSettings { get; set; }

        public NewCaseParams  NewModeParams { get; set; }

        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }

        public DynamicCase DynamicCase { get; set; }

        public int? templateistrue { get; set; }

        public string CaseTemplateName { get; set; }

        public string BackUrl { get; set; }

        public bool CanGetRelatedCases { get; set; }


        #region Date field from case_. Converted to user time zone

        public DateTime RegTime { get; set; }

        #endregion
    }

    public class CaseIndexViewModel
    {
        public int CustomerId { get; set; }
        public CaseSearchFilterData caseSearchFilterData { get; set; }
        public CaseSearchResultModel caseSearchResult { get; set; }
        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }
        public CaseSettingModel CaseSetting { get; set; }
    }

    public class PageSettingsModel
    {
        /// <summary>
        /// Holds values for filter form in case overview page
        /// </summary>
        public JsonCaseSearchFilterData searchFilter { get; set; }

        public JsonGridSettingsModel gridSettings { get; set; }

        public int refreshContent { get; set; }
    }

    public class JsonCaseIndexViewModel
    {       
        public CaseSearchFilterData CaseSearchFilterData { get; set; }

        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }

        public CaseSettingModel CaseSetting { get; set; }

        public PageSettingsModel PageSettings { get; set; }

        public CaseRemainingTimeViewModel RemainingTime { get; set; }
    }

    public class AdvancedSearchIndexViewModel
    {
        public bool DoSearchAtBegining { get; set; }

        public CaseSearchFilterData CaseSearchFilterData { get; set; }

        public AdvancedSearchSpecificFilterData SpecificSearchFilterData { get; set; }

        public CaseTemplateTreeModel CaseTemplateTreeButton { get; set; }

        public CaseSettingModel CaseSetting { get; set; }

        public JsonGridSettingsModel GridSettings { get; set; }

        public CaseRemainingTimeViewModel RemainingTime { get; set; }

        public List<ItemOverview> SelectedCustomers { get; set; }        
    }

    public class CaseSearchResultModel
    {
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