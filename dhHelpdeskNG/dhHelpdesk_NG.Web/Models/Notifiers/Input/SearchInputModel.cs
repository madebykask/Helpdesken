namespace DH.Helpdesk.Web.Models.Notifiers.Input
{
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class SearchInputModel
    {
        public int? DomainId { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? DivisionId { get; set; }

        public string Pharse { get; set; }

        public int RecordsOnPage { get; set; }

        public Enums.Show Show { get; set; }
    }
}