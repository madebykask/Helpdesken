namespace DH.Helpdesk.Web.Models.Notifiers.Input
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class SearchInputModel
    {
        public int? DomainId { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? DivisionId { get; set; }

        public string Pharse { get; set; }

        public int RecordsOnPage { get; set; }

        public NotifierStatus Status { get; set; }

        public SortFieldModel SortField { get; set; }
    }
}