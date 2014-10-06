namespace DH.Helpdesk.Mobile.Models.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.Tools.Concrete;

    public sealed class AverageSolutionTimeReport
    {
        public AverageSolutionTimeReport()
        {
            this.ReportType = ItemOverview.CreateEmpty();
            this.Customer = ItemOverview.CreateEmpty();
            this.Department = ItemOverview.CreateEmpty();
            this.CaseTypes = new ItemOverview[0];
            this.WorkingGroup = ItemOverview.CreateEmpty();
            this.PeriodFrom = DateTime.Today;
            this.PeriodUntil = DateTime.Today;
        }

        public AverageSolutionTimeReport(
                ItemOverview customer, 
                ItemOverview reportType, 
                ItemOverview department, 
                IEnumerable<ItemOverview> caseTypes, 
                ItemOverview workingGroup, 
                DateTime periodFrom, 
                DateTime periodUntil, 
                ReportFile file)
        {
            this.File = file;
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
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

        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ItemOverview WorkingGroup { get; private set; }

        public DateTime PeriodFrom { get; private set; }

        public DateTime PeriodUntil { get; private set; }

        public ReportFile File { get; private set; }
    }
}