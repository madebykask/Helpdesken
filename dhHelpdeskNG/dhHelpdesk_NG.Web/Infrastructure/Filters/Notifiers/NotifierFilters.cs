namespace dhHelpdesk_NG.Web.Infrastructure.Filters.Notifiers
{
    public sealed class NotifierFilters
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