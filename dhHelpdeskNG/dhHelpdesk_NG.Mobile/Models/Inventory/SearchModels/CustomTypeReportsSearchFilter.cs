namespace DH.Helpdesk.Mobile.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

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

        public int? DepartmentId { get; set; }

        public bool IsShowParentInventory { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static CustomTypeReportsSearchFilter CreateDefault()
        {
            return new CustomTypeReportsSearchFilter();
        }
    }
}