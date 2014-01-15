namespace dhHelpdesk_NG.Web.Models.Changes
{
    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Data.Enums.Changes;

    public sealed class SearchModel
    {
        public SearchModel()
        {
        }

        public SearchModel(string pharse, ChangeStatus status, int recordsOnPage)
        {
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
        }

        [Min(0)]
        public int RecordsOnPage { get; set; }

        public string Pharse { get; set; }

        public ChangeStatus Status { get; set; }
    }
}