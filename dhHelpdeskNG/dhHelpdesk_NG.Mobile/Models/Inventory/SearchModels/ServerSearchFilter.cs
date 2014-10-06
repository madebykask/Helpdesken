namespace DH.Helpdesk.Mobile.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Mobile.Models.Shared;

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
            return new ServersFilter(customerId, this.SearchFor);
        }
    }
}