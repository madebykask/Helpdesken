namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class CaseRemainingTimeItemViewModel
    {
        public CaseRemainingTimeItemViewModel(
            int time, 
            int? timeUntil,
            int numberOfCases)
        {
            this.TimeUntil = timeUntil;
            this.NumberOfCases = numberOfCases;
            this.Time = time;
        }

        public int Time { get; private set; }

        public int? TimeUntil { get; private set; }

        public int NumberOfCases { get; private set; }
    }
}