namespace DH.Helpdesk.SelfService.Models.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.SelfService.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.SelfService.Models.Common;

    public sealed class SearchInputModel : ISearchModel<NotifierFilters>
    {
        public int? DomainId { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? DivisionId { get; set; }

        public string Pharse { get; set; }

        public int RecordsOnPage { get; set; }

        public NotifierStatus Status { get; set; }

        public SortFieldModel SortField { get; set; }

        public NotifierFilters ExtractFilters()
        {
            return new NotifierFilters(
                this.DomainId,
                this.RegionId,
                this.DepartmentId,
                this.DivisionId,
                this.Pharse,
                this.Status,
                this.RecordsOnPage,
                this.SortField.Name,
                this.SortField.SortBy ?? SortBy.Ascending);
        }
    }
}