namespace DH.Helpdesk.Services.Requests.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServersFilter
    {
        public ServersFilter(int customerId, string searchFor)
        {
            this.CustomerId = customerId;
            this.SearchFor = searchFor;
        }

        [IsId]
        public int CustomerId { get; private set; }

        public string SearchFor { get; private set; }
    }
}