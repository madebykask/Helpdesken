namespace DH.Helpdesk.BusinessData.Models.Statistics.Output
{
    public sealed class StatisticsOverview
    {
        public StatisticsOverview(
                int activeCases, 
                int endedCases, 
                int inRestCases, 
                int myCases)
        {
            this.MyCases = myCases;
            this.InRestCases = inRestCases;
            this.EndedCases = endedCases;
            this.ActiveCases = activeCases;
        }

        public StatisticsOverview()
        {            
        }

        public int ActiveCases { get; private set; }

        public int EndedCases { get; private set; }

        public int InRestCases { get; private set; }

        public int MyCases { get; private set; }

        public bool IsEmpty()
        {
            return this.ActiveCases == 0 && 
                    this.EndedCases == 0 && 
                    this.InRestCases == 0 &&
                    this.MyCases == 0;
        }
    }
}