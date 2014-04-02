namespace DH.Helpdesk.BusinessData.Models.Statistics.Output
{
    public sealed class StatisticsOverview
    {
        public int ActiveCases { get; set; }
        public int EndedCases { get; set; }
        public int WarningCases { get; set; }

        public bool IsEmpty()
        {
            return ActiveCases == 0 
                   && EndedCases == 0 
                   && WarningCases == 0;
        }
    }
}