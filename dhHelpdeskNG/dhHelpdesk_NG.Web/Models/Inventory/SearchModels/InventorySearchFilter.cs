namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class InventorySearchFilter
    {
        public InventorySearchFilter()
        {
        }

        public InventorySearchFilter(int inventoryTypeId, int? departmentId, string searchFor, int recordsOnPage)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
            this.RecordsOnPage = recordsOnPage;
        }

        private InventorySearchFilter(int inventoryTypeId)
        {
            this.InventoryTypeId = inventoryTypeId;
        }

        [IsId]
        public int InventoryTypeId { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; set; }

        public static InventorySearchFilter CreateDefault(int inventoryTypeId)
        {
            return new InventorySearchFilter(inventoryTypeId);
        }

        public InventoriesFilter CreateRequest()
        {
            return new InventoriesFilter(this.InventoryTypeId, this.DepartmentId, this.SearchFor, this.RecordsOnPage);
        }
    }
}