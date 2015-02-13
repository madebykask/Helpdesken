namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class ReportGeneratorModel
    {
        public ReportGeneratorModel(
            List<GridColumnHeaderModel> headers, 
            List<CaseOverviewModel> cases, 
            int casesFound, 
            SortField sortField)
        {
            this.SortField = sortField;
            this.CasesFound = casesFound;
            this.Cases = cases;
            this.Headers = headers;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; private set; }

        [NotNull]
        public List<CaseOverviewModel> Cases { get; private set; }

        [MinValue(0)]
        public int CasesFound { get; private set; }

        public SortField SortField { get; private set; }
    }
}