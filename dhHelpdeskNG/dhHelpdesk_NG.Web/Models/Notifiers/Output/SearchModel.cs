namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class SearchModel
    {
        public SearchModel(
            SearchDropDownModel domain,
            SearchDropDownModel regions,
            SearchDropDownModel department,
            SearchDropDownModel division,
            string pharse,
            Enums.Show show,
            int recordsOnPage)
        {
            this.Domain = domain;
            this.Region = regions;
            this.Department = department;
            this.Division = division;
            this.Pharse = pharse;
            this.Show = show;
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

        public Enums.Show Show { get; private set; }
    }
}