using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.NewSelfService.Models.Case
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public class NewCaseModel
    {
        public NewCaseModel()
        {

        }

        public NewCaseModel(Case newCase, IList<Region> regions, IList<Department> departments, IList<CaseType> caseTypes,
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
        
}