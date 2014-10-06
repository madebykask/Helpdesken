namespace DH.Helpdesk.Web.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class RegistratedCasesCaseTypeOptions
    {
        public RegistratedCasesCaseTypeOptions()
        {
            this.WorkingGroupIds = new List<int>();
            this.CaseTypeIds = new List<int>();
            this.ProductAreas = new List<ProductArea>();
        }

        public RegistratedCasesCaseTypeOptions(
            MultiSelectList workingGroups, 
            MultiSelectList caseTypes,
            IEnumerable<ProductArea> productAreas, 
            int customerId)
        {
            this.CustomerId = customerId;
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
        
        public int? ProductAreaId { get; set; }

        [MinValue(0)]
        public int CustomerId { get; private set; }

        [LocalizedRequired]
        public DateTime PeriodFrom { get; set; }

        [LocalizedRequired]
        public DateTime PeriodUntil { get; set; }

        public bool ShowDetails { get; set; }

        public bool IsPrint { get; set; }
    }
}