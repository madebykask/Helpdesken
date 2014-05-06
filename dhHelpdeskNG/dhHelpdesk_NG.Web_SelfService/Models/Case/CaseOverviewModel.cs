using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;


namespace DH.Helpdesk.SelfService.Models.Case
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models;

    using Log = DH.Helpdesk.Domain.Log;

    public class SelfServiceModel
    {
        public SelfServiceModel(int customerId, int languageId)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
        }
        
        public int IsEmptyCase { get; set; }

        public int CustomerId { get; set; }

        public int LanguageId { get; set; }              

        public string AUser { get; set; }        

        public string ExLogFileGuid { get; set; }

        public CaseOverviewModel CaseOverview { get; set; }

        public NewCaseModel NewCase { get; set; }

        public UserCasesModel UserCases { get; set; }

        public SelfServiceConfigurationModel Configuration { get; set; }

    }

    public class CaseOverviewModel 
    {
        public CaseOverviewModel()
        { 

        }

        public CaseOverviewModel(string infoText, Case casePreview, List<string> caseFieldGroups, List<Log> caseLogs, List<CaseListToCase> fieldSettings,
                                 IList<Region> regions, IList<System> systems, IList<Supplier> suppliers )
        {
            this.InfoText = infoText;            
            this.CasePreview = casePreview;
            this.CaseFieldGroups = caseFieldGroups;
            this.CaseLogs = caseLogs;
            this.FieldSettings = fieldSettings;
            this.Regions = regions;
            this.Systems = systems;
            this.Suppliers = suppliers;
        }

        public string InfoText { get; set; }        

        public string ReceiptFooterMessage { get; set; }

        public string MailGuid { get; set; }
 
        [StringLength(3000)]
        public string ExtraNote { get; set; }

        public Case CasePreview { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<Log> CaseLogs { get; set; }

        public IList<Region> Regions { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public FilesModel LogFilesModel { get; set; }
                
    }

    public class NewCaseModel
    {        
        public NewCaseModel()
        {

        }

        public NewCaseModel(Case newCase,IList<Region> regions, IList<Department> departments, IList<CaseType> caseTypes,                            
                            IList<ProductArea> productAreas, IList<System> systems, IList<Category> categories,
                            IList<Currency> currencies, IList<Supplier> suppliers, 
                            List<string> caseFieldGroups, List<CaseListToCase> fieldSettings, FilesModel caseFiles, IList<CaseFieldSetting> caseFieldSettings)
        {          
            this.NewCase = newCase;
            this.Regions = regions;
            this.Departments = departments;
            this.CaseTypes = caseTypes;
            this.ProductAreas = productAreas;
            this.Systems = systems;
            this.Categories = categories;
            this.Currencies = currencies;
            this.Suppliers = suppliers;
            this.CaseFieldGroups = caseFieldGroups;            
            this.FieldSettings = fieldSettings;
            this.CaseFilesModel = caseFiles;
            this.CaseFieldSettings = caseFieldSettings;
        }


        public string CaseTypeParantPath { get; set; }

        public string ProductAreaParantPath { get; set; }

        public string CaseFileKey { get; set; }

        public IList<CaseType> CaseTypes { get; set; }

        public IList<ProductArea> ProductAreas { get; set; }

        public Case NewCase { get; set; }

        public IList<Region> Regions { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<Department> Departments { get; set; }

        public IList<Currency> Currencies { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<string> CaseFieldGroups { get; set; }        

        public List<CaseListToCase> FieldSettings { get; set; }

        public FilesModel CaseFilesModel { get; set; }

        public CaseMailSetting CaseMailSetting { get; set; }

        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

    }

    public class UserCasesModel
    {
        public int CustomerId { get; set; }

        public int LanguageId { get; set; } 

        public string UserId { get; set; }

        public string PharasSearch { get; set; }

        public int MaxRecords { get; set; }
       
        public CaseSearchResultModel CaseSearchResult { get; set; }                               
    }

    public class CaseSearchResultModel
    {
        public IList<CaseSettings> CaseSettings { get; set; }

        public IList<CaseSearchResult> Cases { get; set; }

        public CaseColumnsSettingsModel ColumnSettingModel { get; set; }
    }

    public class CaseColumnsSettingsModel
    {        
        public IList<CaseSettings> UserColumns { get; set; }

        public IList<SelectListItem> LineList { get; set; }

        public IList<CaseFieldSettingsWithLanguage> CaseFieldSettingLanguages { get; set; }

    }

    public class SelfServiceConfigurationModel
    {
        public bool ShowNewCase { get; set; }

        public bool ShowUserCases { get; set; }

        public bool ShowOrderAccount { get; set; }

        public bool ShowBulletinBoard { get; set; }

        public bool ShowFAQ { get; set; }

        public bool ShowDashboard { get; set; }

        public int ViewCaseMode { get; set; }

        public bool IsReceipt { get; set; } 
    }
}
