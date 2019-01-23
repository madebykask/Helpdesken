namespace DH.Helpdesk.SelfService.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;
    using Shared;

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
            IList<Urgency> urgencies,
            IList<Impact> impacts,
            IList<Category> categories,
            IList<Currency> currencies,            
            IList<Supplier> suppliers,
            List<string> caseFieldGroups, 
            List<CaseListToCase> fieldSettings, 
            FilesModel caseFiles, 
            IList<CaseFieldSetting> caseFieldSettings,
            JsApplicationOptions jsApplicationOptions,
            IEnumerable<CaseFieldSettingsWithLanguage> caseFieldSettingWithLangauges)
        {
            NewCase = newCase;
            Regions = regions;
            Departments = departments;
            OrganizationUnits = organizationUnits;
            CaseTypes = caseTypes;
            ProductAreas = productAreas;
            Systems = systems;
            Urgencies = urgencies;
            Impacts = impacts;
            Categories = categories;
            Currencies = currencies;
            Suppliers = suppliers;
            CaseFieldGroups = caseFieldGroups;
            FieldSettings = fieldSettings;
            CaseFilesModel = caseFiles;
            CaseFieldSettings = caseFieldSettings;
            JsApplicationOptions = jsApplicationOptions;
            CaseTypeRelatedFields = new List<KeyValuePair<int, string>>();
            CaseFieldSettingWithLangauges = caseFieldSettingWithLangauges;
        }


        public string CaseTypeParantPath { get; set; }

        public string ProductAreaParantPath { get; set; }

        public string CategoryParentPath { get; set; }

        public string CaseFileKey { get; set; }

        public string ExLogFileGuid { get; set; }

        public IList<CaseType> CaseTypes { get; set; }

        public IList<ProductArea> ProductAreas { get; set; }

        public Case NewCase { get; set; }

        public CaseLog CaseLog { get; set; }

        public IList<Region> Regions { get; set; }

        public IList<Department> Departments { get; set; }

        public int DepartmentFilterFormat { get; set; }

        public IList<OU> OrganizationUnits { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Urgency> Urgencies { get; set; }

        public IList<Impact> Impacts { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<Currency> Currencies { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public FilesModel CaseFilesModel { get; set; }

        public CaseMailSetting CaseMailSetting { get; set; }

        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }

        public JsApplicationOptions JsApplicationOptions { get; set; }

        public List<FieldSettingJSModel> JsFieldSettings { get; set; }

        public List<ProductAreaChild> ProductAreaChildren { get; set; }

        public string FollowerUsers { get; set; }

        public SendToDialogModel SendToDialogModel { get; set; }

        public List<KeyValuePair<int,string>> CaseTypeRelatedFields { get; set; }

        public string Information { get; set; }

        public Customer CurrentCustomer { get; set; }
    }


    public sealed class ProductAreaChild
    {
        public ProductAreaChild(int productAreaId, bool hasChildren)
        {
            ProductAreaId = productAreaId;
            HasChildren = hasChildren;
        }

        public int ProductAreaId { get; private set; }

        public bool HasChildren { get; private set; }
    }
}