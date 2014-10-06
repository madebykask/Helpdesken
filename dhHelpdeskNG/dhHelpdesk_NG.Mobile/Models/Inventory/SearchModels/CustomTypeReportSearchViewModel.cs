namespace DH.Helpdesk.Mobile.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CustomTypeReportSearchViewModel
    {
        public CustomTypeReportSearchViewModel(
            SelectList departments,
            CustomTypeReportsSearchFilter filter, 
            int inventoryTypeId)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.Departments = departments;
            this.Filter = filter;
        }

        [NotNull]
        public CustomTypeReportsSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        public static CustomTypeReportSearchViewModel BuildViewModel(
            CustomTypeReportsSearchFilter currentFilter,
            List<ItemOverview> departments,
            int inventoryTypeId)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new CustomTypeReportSearchViewModel(departmentsSelectList, currentFilter, inventoryTypeId);
        }
    }
}