namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    public sealed class CaseRemainingTimeData
    {
        private readonly List<CaseRemainingTime> caseRemainingTimes = new List<CaseRemainingTime>();

        public List<CaseRemainingTime> CaseRemainingTimes
        {
            get
            {
                return this.caseRemainingTimes;
            }
        }

        public void AddRemainingTime(int caseId, int remainingTime)
        {
            this.caseRemainingTimes.Add(new CaseRemainingTime(caseId, remainingTime));
        }
    }
}