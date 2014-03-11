namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchModel
    {
        public SearchModel(
            SearchDropDownModel domain,
            SearchDropDownModel regions,
            SearchDropDownModel department,
            SearchDropDownModel division,
            string pharse,
            NotifierStatus status,
            int recordsOnPage)
        {
            this.Domain = domain;
            this.Region = regions;
            this.Department = department;
            this.Division = division;
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
        }

        [NotNull]
        public SearchDropDownModel Domain { get; private set; }

        [NotNull]
        public SearchDropDownModel Region { get; private set; }

        [NotNull]
        public SearchDropDownModel Department { get; private set; }

        [NotNull]
        public SearchDropDownModel Division { get; private set; }

        public string Pharse { get; private set; }

        public int RecordsOnPage { get; private set; }

        public NotifierStatus Status { get; private set; }
    }
}