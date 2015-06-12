namespace DH.Helpdesk.NewSelfService.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public class JsApplicationOptions
    {
        public int customerId;

        public int departmentFilterFormat;

        public string departmentsURL;

        public string orgUnitURL;
    }

    public class NewCaseModel
    {
        public NewCaseModel(
            Case newCase,
            IList<Region> regions,
            IList<Department> departments,
            IList<OU> organizationUnits,
            IList<CaseType> caseTypes,
            IList<ProductArea> productAreas,
            IList<System> systems,
            IList<Category> categories,
            IList<Currency> currencies, 
            IList<Supplier> suppliers,
            List<string> caseFieldGroups, 
            List<CaseListToCase> fieldSettings, 
            FilesModel caseFiles, 
            IList<CaseFieldSetting> caseFieldSettings,
            JsApplicationOptions jsApplicationOptions)
        {
            this.NewCase = newCase;
            this.Regions = regions;
            this.Departments = departments;
            this.OrganizationUnits = organizationUnits;
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
            this.JsApplicationOptions = jsApplicationOptions;
        }


        public string CaseTypeParantPath { get; set; }

        public string ProductAreaParantPath { get; set; }

        public string CaseFileKey { get; set; }

        public string ExLogFileGuid { get; set; }

        public IList<CaseType> CaseTypes { get; set; }

        public IList<ProductArea> ProductAreas { get; set; }

        public Case NewCase { get; set; }

        public IList<Region> Regions { get; set; }

        public IList<Department> Departments { get; set; }

        public int DepartmentFilterFormat { get; set; }

        public IList<OU> OrganizationUnits { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<Currency> Currencies { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public FilesModel CaseFilesModel { get; set; }

        public CaseMailSetting CaseMailSetting { get; set; }

        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

        public JsApplicationOptions JsApplicationOptions { get; set; }
    }
}