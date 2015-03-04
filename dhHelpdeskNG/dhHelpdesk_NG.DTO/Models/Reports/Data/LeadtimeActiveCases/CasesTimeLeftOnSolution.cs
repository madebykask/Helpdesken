namespace DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CasesTimeLeftOnSolution
    {
        public CasesTimeLeftOnSolution(
            int time, 
            int numberOfCases)
        {
            this.NumberOfCases = numberOfCases;
            this.Time = time;
        }

        [MinValue(0)]
        public int Time { get; private set; }

        [MinValue(0)]
        public int NumberOfCases { get; private set; }
    }
}