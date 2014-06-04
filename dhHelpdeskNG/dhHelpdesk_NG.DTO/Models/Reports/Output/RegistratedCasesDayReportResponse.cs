namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistratedCasesDayReportResponse
    {
        public RegistratedCasesDayReportResponse()
        {
            this.Items = new RegistratedCasesDayItem[0];
            this.ReportType = ItemOverview.CreateEmpty();
            this.Customer = ItemOverview.CreateEmpty();
            this.CaseTypes = new ItemOverview[0];
        }

        public RegistratedCasesDayReportResponse(
                    ItemOverview customer, 
                    ItemOverview reportType, 
                    ItemOverview department, 
                    IEnumerable<ItemOverview> caseTypes, 
                    ItemOverview workingGroup, 
                    ItemOverview administrator, 
                    IEnumerable<RegistratedCasesDayItem> items)
        {
            this.Items = items;
            this.Administrator = administrator;
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

        public ItemOverview Administrator { get; private set; }

        [NotNull]
        public IEnumerable<RegistratedCasesDayItem> Items { get; private set; } 
    }
}