using DH.Helpdesk.Web.Enums.Inventory;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
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

        public int? DepartmentId { get; set; }

        public bool IsShowParentInventory { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static CustomTypeReportsSearchFilter CreateDefault()
        {
            return new CustomTypeReportsSearchFilter();
        }

        public static string CreateFilterId()
        {
            return $"{TabName.Reports}{ReportFilterMode.CustomType}";
        }
    }
}