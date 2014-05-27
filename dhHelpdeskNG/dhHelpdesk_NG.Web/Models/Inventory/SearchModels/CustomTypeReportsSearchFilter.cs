namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class CustomTypeReportsSearchFilter
    {
        public CustomTypeReportsSearchFilter()
        {
        }

        public CustomTypeReportsSearchFilter(
            int? departmentId,
            bool isShowParentInventory,
            string searchFor)
        {
            this.DepartmentId = departmentId;
            this.IsShowParentInventory = isShowParentInventory;
            this.SearchFor = searchFor;
        }

        private CustomTypeReportsSearchFilter(int inventoryTypeId)
        {
            this.InventoryTypeId = inventoryTypeId;
        }

        public int InventoryTypeId { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsShowParentInventory { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static CustomTypeReportsSearchFilter CreateDefault(int inventoryTypeId)
        {
            return new CustomTypeReportsSearchFilter(inventoryTypeId);
        }
    }
}