namespace DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ClosedCasesDayData
    {
        public ClosedCasesDayData(List<ClosedCasesDayCase> closedCases)
        {
            this.ClosedCases = closedCases;
        }

        [NotNull]
        public List<ClosedCasesDayCase> ClosedCases { get; private set; }
    }
}