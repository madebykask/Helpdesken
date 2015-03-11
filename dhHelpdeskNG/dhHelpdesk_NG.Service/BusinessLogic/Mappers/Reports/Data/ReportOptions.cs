namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Reports.Data
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    internal sealed class ReportOptions
    {
        public ReportOptions(
            List<ItemOverview> departments, 
            List<ItemOverview> caseTypesOverviews, 
            List<CaseTypeItem> caseTypes, 
            List<ProductAreaItem> productAreas, 
            List<ItemOverview> workingGroups, 
            List<ItemOverview> administrators, 
            List<ItemOverview> fields)
        {
            this.Fields = fields;
            this.Administrators = administrators;
            this.WorkingGroups = workingGroups;
            this.ProductAreas = productAreas;
            this.CaseTypes = caseTypes;
            this.CaseTypesOverviews = caseTypesOverviews;
            this.Departments = departments;
        }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }

        [NotNull]
        public List<ItemOverview> CaseTypesOverviews { get; private set; }

        [NotNull]
        public List<CaseTypeItem> CaseTypes { get; private set; }

        [NotNull]
        public List<ProductAreaItem> ProductAreas { get; private set; }

        [NotNull]
        public List<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }

        [NotNull]
        public List<ItemOverview> Fields { get; private set; } 
    }
}