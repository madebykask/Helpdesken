namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public class ServerSearchFilter
    {
        public ServerSearchFilter(int customerId, string searchFor)
        {
            this.CustomerId = customerId;
            this.SearchFor = searchFor;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }
    }
}