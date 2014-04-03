using System;
using System;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;


namespace DH.Helpdesk.SelfService.Models.Case
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models;

    using Log = DH.Helpdesk.Domain.Log;

    public class SelfServiceModel
    {
        public SelfServiceModel(int languageId)
        {
            this.LanguageId = languageId;
        }
        
        public int CustomerId { get; set; }

        public int LanguageId { get; set; }
       
        public CaseOverviewModel CaseOverview { get; set; }

        public NewCaseModel NewCase { get; set; }
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
        private readonly IList<CaseType> caseTypes;

        public NewCaseModel()
        {

        }

        public NewCaseModel(Case newCase,IList<Region> regions, IList<Department> departments, IList<CaseType> caseTypes,                            
                            IList<ProductArea> productAreas, IList<System> systems, IList<Category> categories,
                            IList<Currency> currencies, IList<Supplier> suppliers,
                            List<string> caseFieldGroups, List<CaseListToCase> fieldSettings, FilesModel caseFiles)
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

    }
}
