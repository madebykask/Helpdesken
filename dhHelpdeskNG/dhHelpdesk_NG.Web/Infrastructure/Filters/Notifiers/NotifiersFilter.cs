namespace DH.Helpdesk.Web.Infrastructure.Filters.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Common.Enums;

    public sealed class NotifiersFilter
    {
        public int? DomainId { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? DivisionId { get; set; }

        public string Pharse { get; set; }

        public NotifierStatus Status { get; set; }

        public int RecordsOnPage { get; set; }

        public string SortByField { get; set; }

        public SortBy SortBy { get; set; }
    }
}