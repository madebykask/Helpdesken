namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class RegistratedCasesCaseTypeResponsePrint
    {
        public RegistratedCasesCaseTypeResponsePrint()
        {
            this.WorkingGroups = new ItemOverview[] { };
            this.CaseTypes = new ItemOverview[] { };
        }

        public RegistratedCasesCaseTypeResponsePrint(
                IEnumerable<ItemOverview> workingGroups, 
                IEnumerable<ItemOverview> caseTypes, 
                ProductArea productArea)
        {
            this.ProductArea = productArea;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
        }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ProductArea ProductArea { get; private set; }
    }
}