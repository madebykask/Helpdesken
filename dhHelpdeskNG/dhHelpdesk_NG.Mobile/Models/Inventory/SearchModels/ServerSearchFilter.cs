namespace DH.Helpdesk.Mobile.Models.Inventory.SearchModels
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Mobile.Models.Shared;
    using DH.Helpdesk.Services.Requests.Inventory;

    public class ServerSearchFilter
    {
        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public SortFieldModel SortField { get; set; }

        public static ServerSearchFilter CreateDefault()
        {
            return new ServerSearchFilter { SortField = new SortFieldModel() };
        }

        public ServersFilter CreateRequest(int customerId)
        {
            var sf = new SortField(this.SortField.Name, this.SortField.SortBy ?? SortBy.Ascending);
            return new ServersFilter(customerId, this.SearchFor, sf);
        }
    }
}