namespace DH.Helpdesk.Web.Infrastructure.Filters.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NotifierFilters
    {
        private NotifierFilters()
        {
        }

        public NotifierFilters(
            int? domainId,
            int? regionId,
            int? departmentId,
            int? divisionId,
            string pharse,
            NotifierStatus status,
            int recordsOnPage,
            string sortByField,
            SortBy sortBy)
        {
            this.DomainId = domainId;
            this.RegionId = regionId;
            this.DepartmentId = departmentId;
            this.DivisionId = divisionId;
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
            this.SortByField = sortByField;
            this.SortBy = sortBy;
        }

        [IsId]
        public int? DomainId { get; private set; }

        [IsId]
        public int? RegionId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? DivisionId { get; private set; }

        public string Pharse { get; private set; }

        public NotifierStatus Status { get; private set; }

        [MinValue(0)]
        public int RecordsOnPage { get; private set; }

        public string SortByField { get; private set; }

        public SortBy SortBy { get; private set; }

        public static NotifierFilters CreateDefault()
        {
            return new NotifierFilters { Status = NotifierStatus.Active, RecordsOnPage = 100 };
        }
    }
}