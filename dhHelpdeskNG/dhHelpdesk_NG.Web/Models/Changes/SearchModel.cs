namespace dhHelpdesk_NG.Web.Models.Changes
{
    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Web.Infrastructure;

    public sealed class SearchModel
    {
        public SearchModel(string pharse, Enums.Show show, int recordsOnPage)
        {
            this.Pharse = pharse;
            this.Show = show;
            this.RecordsOnPage = recordsOnPage;
        }

        [Min(0)]
        public int RecordsOnPage { get; set; }

        public string Pharse { get; set; }

        public Enums.Show Show { get; set; }
    }
}