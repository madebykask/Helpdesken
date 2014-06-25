namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AverageSolutionTimeReportResponse
    {
        public AverageSolutionTimeReportResponse()
        {
            this.ReportType = ItemOverview.CreateEmpty();
            this.Customer = ItemOverview.CreateEmpty();
            this.CaseTypes = new ItemOverview[0];
        }

        public AverageSolutionTimeReportResponse(
                    ItemOverview customer, 
                    ItemOverview reportType, 
                    ItemOverview department, 
                    IEnumerable<ItemOverview> caseTypes, 
                    ItemOverview workingGroup)
        {
            this.WorkingGroup = workingGroup;
            this.CaseTypes = caseTypes;
            this.Department = department;
            this.ReportType = reportType;
            this.Customer = customer;
        }

        [NotNull]
        public ItemOverview Customer { get; private set; }

        [NotNull]
        public ItemOverview ReportType { get; private set; }

        public ItemOverview Department { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ItemOverview WorkingGroup { get; private set; }         
    }
}