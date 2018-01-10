namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class ServerSearchFilter
    {
        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public SortFieldModel SortField { get; set; }

        public int? RecordsCount { get; set; }

        public static ServerSearchFilter CreateDefault()
        {
            return new ServerSearchFilter { SortField = new SortFieldModel() };
        }

        public ServersFilter CreateRequest(int customerId)
        {
            SortField sf = null;
            if (!string.IsNullOrEmpty(this.SortField.Name))
            {
                sf = new SortField(this.SortField.Name, this.SortField.SortBy ?? SortBy.Ascending);
            }

            return new ServersFilter(customerId, this.SearchFor, sf, this.RecordsCount);
        }
    }
}