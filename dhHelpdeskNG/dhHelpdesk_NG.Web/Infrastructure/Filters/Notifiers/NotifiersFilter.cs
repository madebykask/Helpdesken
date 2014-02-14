namespace DH.Helpdesk.Web.Infrastructure.Filters.Notifiers
{
    public sealed class NotifiersFilter
    {
        public int? DomainId { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? DivisionId { get; set; }

        public string Pharse { get; set; }

        public Enums.Show Show { get; set; }

        public int RecordsOnPage { get; set; }
    }
}