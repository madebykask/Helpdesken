namespace DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReportGeneratorData
    {
        public ReportGeneratorData(
            FullCaseSettings settings, 
            List<FullCaseOverview> cases)
        {
            this.Cases = cases;
            this.Settings = settings;
        }

        [NotNull]
        public FullCaseSettings Settings { get; private set; }

        [NotNull]
        public List<FullCaseOverview> Cases { get; private set; }
    }
}