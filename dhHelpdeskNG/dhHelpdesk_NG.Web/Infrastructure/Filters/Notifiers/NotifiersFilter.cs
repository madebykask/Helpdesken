namespace DH.Helpdesk.Web.Infrastructure.Filters.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;

    public sealed class NotifiersFilter
    {
        public int? DomainId { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? DivisionId { get; set; }

        public string Pharse { get; set; }

        public NotifierStatus Status { get; set; }

        public int RecordsOnPage { get; set; }
    }
}