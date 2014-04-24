namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ServerSearchFilter
    {
        public ServerSearchFilter()
        {
        }

        public ServerSearchFilter(string searchFor)
        {
            this.SearchFor = searchFor;
        }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static ServerSearchFilter CreateDefault()
        {
            return new ServerSearchFilter();
        }

        public ServersFilter CreateRequest(int customerId)
        {
            return new ServersFilter(customerId, this.SearchFor);
        }
    }
}