namespace DH.Helpdesk.Web.Models.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class SearchModel
    {
        public SearchModel(
            SearchDropDownModel domain,
            SearchDropDownModel regions,
            SearchDropDownModel department,
            SearchDropDownModel organizationUnit,
            SearchDropDownModel division,
			SearchDropDownModel computerUserCategories,
			string pharse,
            NotifierStatus status,
            int recordsOnPage,
            SortFieldModel sortField)
        {
            this.Domain = domain;
            this.Region = regions;
            this.Department = department;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
			this.ComputerUserCategories = computerUserCategories;
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
        public SearchDropDownModel OrganizationUnit { get; private set; }

        [NotNull]
        public SearchDropDownModel Division { get; private set; }

        public string Pharse { get; private set; }

        public int RecordsOnPage { get; private set; }

        public NotifierStatus Status { get; private set; }

        public SortFieldModel SortField { get; private set; }

		public SearchDropDownModel ComputerUserCategories { get; private set; }
	}
}