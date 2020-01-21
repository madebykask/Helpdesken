namespace DH.Helpdesk.BusinessData.Models.Statistics.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class StatisticsOverview
    {
        public StatisticsOverview()
        {
        }

        [MinValue(0)]
        public int InProgress { get; set; }

        [MinValue(0)]
        public int Unopened { get; set; }

        [MinValue(0)]
        public int Onhold { get; set; }

        [MinValue(0)]
        public int Overdue { get; set; }

        [MinValue(0)]
        public int NewToday { get; set; }

        [MinValue(0)]
        public int DueToday { get; set; }

        [MinValue(0)]
        public int SolvedToday { get; set; }

        [MinValue(0)]
        public int SolvedInTimeToday { get; set; }
    }
}