namespace DH.Helpdesk.Services.Requests.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrintersFilter
    {
        public PrintersFilter(int customerId, int? departmentId, string searchFor, int? recordsCount)
        {
            this.CustomerId = customerId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
            this.RecordsCount = recordsCount;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        public string SearchFor { get; private set; }

        public int? RecordsCount { get; set; }
    }
}
