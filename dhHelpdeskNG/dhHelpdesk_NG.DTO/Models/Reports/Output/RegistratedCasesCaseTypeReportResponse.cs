namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class RegistratedCasesCaseTypeReportResponse
    {
        public RegistratedCasesCaseTypeReportResponse()
        {
            this.ReportType = ItemOverview.CreateEmpty();
            this.Customer = ItemOverview.CreateEmpty();
            this.WorkingGroups = new ItemOverview[] { };
            this.CaseTypes = new ItemOverview[] { };
            this.ProductArea = new ProductArea();
        }

        public RegistratedCasesCaseTypeReportResponse(
                ItemOverview customer,
                ItemOverview reportType,
                IEnumerable<ItemOverview> workingGroups, 
                IEnumerable<ItemOverview> caseTypes, 
                ProductArea productArea)
        {
            this.ReportType = reportType;
            this.Customer = customer;
            this.ProductArea = productArea;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
        }

        [NotNull]
        public ItemOverview Customer { get; private set; }

        [NotNull]
        public ItemOverview ReportType { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ProductArea ProductArea { get; private set; }
    }
}