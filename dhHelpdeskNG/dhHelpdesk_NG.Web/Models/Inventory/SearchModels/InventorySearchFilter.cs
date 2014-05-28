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

        public InventorySearchFilter(int? departmentId, string searchFor, int recordsOnPage)
        {
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
            this.RecordsOnPage = recordsOnPage;
        }

        private InventorySearchFilter(int recordsOnPage)
        {
            this.RecordsOnPage = recordsOnPage;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; set; }

        public static InventorySearchFilter CreateDefault(int inventoryTypeId)
        {
            return new InventorySearchFilter(500);
        }

        public InventoriesFilter CreateRequest(int inventoryTypeId)
        {
            return new InventoriesFilter(inventoryTypeId, this.DepartmentId, this.SearchFor, this.RecordsOnPage);
        }
    }
}