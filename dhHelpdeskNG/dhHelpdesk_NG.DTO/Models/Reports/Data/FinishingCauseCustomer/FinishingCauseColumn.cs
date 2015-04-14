namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer
{
    using System.Collections.Generic;

    public sealed class FinishingCauseColumn
    {
        public FinishingCauseColumn(
            int casesNumber, 
            double caseNumberPercents, 
            List<int> caseIds)
        {
            this.CaseIds = caseIds;
            this.CaseNumberPercents = caseNumberPercents;
            this.CasesNumber = casesNumber;
        }

        public int CasesNumber { get; private set; }

        public double CaseNumberPercents { get; private set; }

        public List<int> CaseIds { get; private set; } 
    }
}