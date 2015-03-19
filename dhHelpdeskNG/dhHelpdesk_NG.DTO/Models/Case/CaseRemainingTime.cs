namespace DH.Helpdesk.BusinessData.Models.Case
{
    public sealed class CaseRemainingTime
    {
        public CaseRemainingTime(
            int caseId, 
            int remainingTime)
        {
            this.RemainingTime = remainingTime;
            this.CaseId = caseId;
        }

        public int CaseId { get; private set; }

        public int RemainingTime { get; private set; }
    }
}