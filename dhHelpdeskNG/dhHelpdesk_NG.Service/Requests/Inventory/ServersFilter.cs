namespace DH.Helpdesk.Services.Requests.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServersFilter
    {
        public ServersFilter(int customerId, string searchFor, SortField sortField, int? recordsCount)
        {
            this.CustomerId = customerId;
            this.SearchFor = searchFor;
            this.SortField = sortField;
            this.RecordsCount = recordsCount;
        }

        [IsId]
        public int CustomerId { get; private set; }

        public string SearchFor { get; private set; }

        public SortField SortField { get; private set; }

        public int? RecordsCount { get; set; }
    }
}