namespace DH.Helpdesk.Web.Models.Reports
{
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    public class CaseSatisfactionReport
    {
        public CaseSatisfactionReport(int goodVotes, int normalVotes, int badVotes, int count, ReportFile file)
        {
            this.GoodVotes = goodVotes;
            this.NormalVotes = normalVotes;
            this.BadVotes = badVotes;
            this.Count = count;
            this.File = file;
        }

        public ReportFile File { get; set; }

        public int GoodVotes { get; private set; }

        public int NormalVotes { get; private set; }

        public int BadVotes { get; private set; }

        public int Count { get; private set; }
    }
}