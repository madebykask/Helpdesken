namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class InventorySearchFilter
    {
        public InventorySearchFilter()
        {
        }

        private InventorySearchFilter(int recordsOnPage)
        {
            this.RecordsOnPage = recordsOnPage;

            this.SortField = new SortFieldModel();
        }

        [IsId]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; set; }

        public SortFieldModel SortField { get; set; }

        public static InventorySearchFilter CreateDefault(int inventoryTypeId) // todo wtf?
        {
            return new InventorySearchFilter(Helpdesk.Common.Constants.SearchFilter.RecordsOnPage);
        }

        public InventoriesFilter CreateRequest(int inventoryTypeId, int? numberOfRecords = null)
        {
            return new InventoriesFilter(inventoryTypeId, DepartmentId, SearchFor, numberOfRecords?? RecordsOnPage);
        }
    }
}