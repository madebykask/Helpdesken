namespace DH.Helpdesk.BusinessData.Models.Statistics.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class StatisticsOverview
    {
        public StatisticsOverview(
                int activeCases, 
                int endedCases, 
                int inRestCases, 
                int myCases, 
                int unreadCases)
        {
            this.UnreadCases = unreadCases;
            this.MyCases = myCases;
            this.InRestCases = inRestCases;
            this.EndedCases = endedCases;
            this.ActiveCases = activeCases;
        }

        public StatisticsOverview()
        {
        }

        [MinValue(0)]
        public int ActiveCases { get; private set; }

        [MinValue(0)]
        public int UnreadCases { get; private set; }

        [MinValue(0)]
        public int EndedCases { get; private set; }

        [MinValue(0)]
        public int InRestCases { get; private set; }

        [MinValue(0)]
        public int MyCases { get; private set; }

        public bool IsEmpty()
        {
            return this.ActiveCases == 0 && 
                    this.UnreadCases == 0 &&
                    this.EndedCases == 0 && 
                    this.InRestCases == 0 &&
                    this.MyCases == 0;
        }
    }
}