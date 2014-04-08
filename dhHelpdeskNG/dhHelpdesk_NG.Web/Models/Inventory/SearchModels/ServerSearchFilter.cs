namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ServerSearchFilter
    {
        public ServerSearchFilter(int customerId, string searchFor)
        {
            this.CustomerId = customerId;
            this.SearchFor = searchFor;
        }

        private ServerSearchFilter(int customerId)
        {
            this.CustomerId = customerId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }

        public static ServerSearchFilter CreateDefault(int customerId)
        {
            return new ServerSearchFilter(customerId);
        }

        public ServersFilter CreateRequest()
        {
            return new ServersFilter(this.CustomerId, this.SearchFor);
        }
    }
}