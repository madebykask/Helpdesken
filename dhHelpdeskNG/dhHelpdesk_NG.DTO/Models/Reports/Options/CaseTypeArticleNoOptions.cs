namespace DH.Helpdesk.BusinessData.Models.Reports.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseTypeArticleNoOptions
    {
        public CaseTypeArticleNoOptions(
                List<ItemOverview> departments, 
                List<ItemOverview> workingGroups, 
                List<ItemOverview> caseTypes, 
                List<ProductAreaItem> productAreas)
        {
            this.ProductAreas = productAreas;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
        }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }

        [NotNull]
        public List<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> CaseTypes { get; private set; }

        [NotNull]
        public List<ProductAreaItem> ProductAreas { get; private set; }
    }
}