namespace DH.Helpdesk.Web.Models.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    public sealed class RegistratedCasesCaseTypeReport
    {
        public RegistratedCasesCaseTypeReport()
        {
            this.ReportType = ItemOverview.CreateEmpty();
            this.Customer = ItemOverview.CreateEmpty();
            this.Files = new List<ReportFile>();
            this.WorkingGroups = new ItemOverview[0];
            this.CaseTypes = new ItemOverview[0];
        }

        public RegistratedCasesCaseTypeReport(
                        ItemOverview customer,
                        ItemOverview reportType,
                        IEnumerable<ItemOverview> workingGroups, 
                        IEnumerable<ItemOverview> caseTypes, 
                        ProductArea productArea, 
                        DateTime periodFrom, 
                        DateTime periodUntil,
                        List<ReportFile> files)
        {
            this.ReportType = reportType;
            this.Customer = customer;
            this.Files = files;
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
            this.ProductArea = productArea;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
        }

        [NotNull]
        public ItemOverview Customer { get; private set; }

        [NotNull]
        public ItemOverview ReportType { get; private set; }

        [NotNull]
        public List<ReportFile> Files { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        public ProductArea ProductArea { get; private set; }

        public DateTime PeriodFrom { get; private set; }

        public DateTime PeriodUntil { get; private set; }
    }
}