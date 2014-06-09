namespace DH.Helpdesk.Web.Models.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    public sealed class RegistratedCasesDayReport
    {
        public RegistratedCasesDayReport()
        {
            this.ReportType = ItemOverview.CreateEmpty();
            this.Customer = ItemOverview.CreateEmpty();
            this.Department = ItemOverview.CreateEmpty();
            this.CaseTypes = new ItemOverview[0];
            this.WorkingGroup = ItemOverview.CreateEmpty();
            this.Administrator = ItemOverview.CreateEmpty();
            this.Period = DateTime.Today;
        }

        public RegistratedCasesDayReport(
            ItemOverview customer, 
            ItemOverview reportType, 
            ItemOverview department,
            IEnumerable<ItemOverview> caseTypes, 
            ItemOverview workingGroup, 
            ItemOverview administrator,
            DateTime period,
            ReportFile file)
        {
            this.File = file;
            this.Administrator = administrator;
            this.WorkingGroup = workingGroup;
            this.CaseTypes = caseTypes;
            this.Department = department;
            this.ReportType = reportType;
            this.Period = period;
            this.Customer = customer;
        }

        [NotNull]
        public ItemOverview Customer { get; private set; }

        [NotNull]
        public ItemOverview ReportType { get; private set; }

        public ItemOverview Department { get; private set; }

        public IEnumerable<ItemOverview> CaseTypes { get; private set; }
        
        public ItemOverview WorkingGroup { get; private set; }

        public ItemOverview Administrator { get; private set; }

        public DateTime Period { get; private set; }

        public ReportFile File { get; private set; }
    }
}