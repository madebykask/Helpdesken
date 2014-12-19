namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    public class CaseSatisfactionReportResponse
    {
        public CaseSatisfactionReportResponse(
            int goodVotes,
            int normalVotes,
            int badVotes,
            int count)
        {
            this.GoodVotes = goodVotes;
            this.NormalVotes = normalVotes;
            this.BadVotes = badVotes;
            this.Count = count;
        }
      
        public int GoodVotes { get; private set; }

        public int NormalVotes { get; private set; }

        public int BadVotes { get; private set; }

        public int Count { get; private set; }
    }
}