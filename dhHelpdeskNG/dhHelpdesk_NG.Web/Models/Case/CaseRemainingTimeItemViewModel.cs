namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class CaseRemainingTimeItemViewModel
    {
        public CaseRemainingTimeItemViewModel(
            int time, 
            int numberOfCases)
        {
            this.NumberOfCases = numberOfCases;
            this.Time = time;
        }

        public int Time { get; private set; }

        public int NumberOfCases { get; private set; }
    }
}