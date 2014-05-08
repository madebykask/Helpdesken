namespace DH.Helpdesk.BusinessData.Models.Statistics.Output
{
    public sealed class StatisticsOverview
    {
        public int ActiveCases { get; set; }

        public int EndedCases { get; set; }

        public int InRestCases { get; set; }

        public int MyCases { get; set; }

        public bool IsEmpty()
        {
            return this.ActiveCases == 0 && 
                    this.EndedCases == 0 && 
                    this.InRestCases == 0 &&
                    this.MyCases == 0;
        }
    }
}