namespace DH.Helpdesk.NewSelfService.Models.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.NewSelfService.Models.Common;

    public sealed class SearchModel
    {
        public SearchModel(
            SearchDropDownModel domain,
            SearchDropDownModel regions,
            SearchDropDownModel department,
            SearchDropDownModel division,
            string pharse,
            NotifierStatus status,
            int recordsOnPage,
            SortFieldModel sortField)
        {
            this.Domain = domain;
            this.Region = regions;
            this.Department = department;
            this.Division = division;
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
            this.SortField = sortField;
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

        public SortFieldModel SortField { get; private set; }
    }
}