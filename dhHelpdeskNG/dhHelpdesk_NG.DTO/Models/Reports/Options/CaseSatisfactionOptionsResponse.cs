﻿namespace DH.Helpdesk.BusinessData.Models.Reports.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public class CaseSatisfactionOptionsResponse
    {
        public CaseSatisfactionOptionsResponse()
        {
            this.WorkingGroups = new ItemOverview[] { };
            this.CaseTypes = new ItemOverview[] { };
            this.ProductAreas = new ProductArea[] { };
        }

        public CaseSatisfactionOptionsResponse(
            IEnumerable<ItemOverview> workingGroups,
            IEnumerable<ItemOverview> caseTypes,
            IEnumerable<ProductArea> productAreas)
        {
            this.ProductAreas = productAreas;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
        }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        [NotNull]
        public IEnumerable<ProductArea> ProductAreas { get; private set; } 
    }
}