namespace DH.Helpdesk.Services.Requests.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoriesFilter
    {
        public InventoriesFilter(int inventoryTypeId, int? departmentId, string searchFor, int recordsOnPage)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
            this.RecordsOnPage = recordsOnPage;
        }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        public string SearchFor { get; private set; }

        [MinValue(-1)]
        public int RecordsOnPage { get; private set; }
    }
}
