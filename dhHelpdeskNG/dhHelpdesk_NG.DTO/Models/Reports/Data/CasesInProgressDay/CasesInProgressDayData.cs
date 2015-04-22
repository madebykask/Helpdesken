namespace DH.Helpdesk.BusinessData.Models.Reports.Data.CasesInProgressDay
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CasesInProgressDayData
    {
        public CasesInProgressDayData(List<CasesInProgressDayCase> cases)
        {
            this.Cases = cases;
        }

        [NotNull]
        public List<CasesInProgressDayCase> Cases { get; private set; }
    }
}