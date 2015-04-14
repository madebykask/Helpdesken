namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.FinishingCause;

    public sealed class FinishingCauseRow
    {
        public FinishingCauseRow(
            FinishingCauseItem finishingCause, 
            List<FinishingCauseColumn> columns)
        {
            this.Columns = columns;
            this.FinishingCause = finishingCause;
        }

        public FinishingCauseItem FinishingCause { get; private set; }

        public List<FinishingCauseColumn> Columns { get; private set; } 
    }
}