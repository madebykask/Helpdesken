namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class CaseRemainingTimeItemViewModel
    {
        public CaseRemainingTimeItemViewModel(
            int id,
            int time, 
            int? timeUntil,
            int numberOfCases, 
            string tooltip,
            string caption)
        {
            this.Id = id;
            this.Tooltip = tooltip;
            this.TimeUntil = timeUntil;
            this.NumberOfCases = numberOfCases;
            this.Time = time;
            this.Caption = caption;
        }

        public CaseRemainingTimeItemViewModel(
            int id,
            int time, 
            int? timeUntil,
            int numberOfCases,
            string caption) : this(id, time, timeUntil, numberOfCases, string.Empty, caption)
        {
        }

        public int Id { get; private set; }

        public int Time { get; private set; }

        public int? TimeUntil { get; private set; }

        public int NumberOfCases { get; private set; }

        public string Tooltip { get; private set; }

        public string Caption { get; private set; }
    }
}