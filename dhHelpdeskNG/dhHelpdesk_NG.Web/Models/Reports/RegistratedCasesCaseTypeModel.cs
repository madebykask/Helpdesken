namespace DH.Helpdesk.Web.Models.Reports
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class RegistratedCasesCaseTypeModel
    {
        public RegistratedCasesCaseTypeModel()
        {
            this.WorkingGroupIds = new List<int>();
            this.CaseTypeIds = new List<int>();
            this.ProductAreas = new List<ProductArea>();
        }

        public RegistratedCasesCaseTypeModel(
            MultiSelectList workingGroups, 
            MultiSelectList caseTypes,
            IEnumerable<ProductArea> productAreas)
        {
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.ProductAreas = productAreas;
        }

        [NotNull]
        public MultiSelectList WorkingGroups { get; private set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public MultiSelectList CaseTypes { get; private set; }

        [NotNull]
        public List<int> CaseTypeIds { get; set; } 

        [NotNull]
        public IEnumerable<ProductArea> ProductAreas { get; private set; }
        
        [MinValue(0)]
        public int ProductAreaId { get; set; }
    }
}