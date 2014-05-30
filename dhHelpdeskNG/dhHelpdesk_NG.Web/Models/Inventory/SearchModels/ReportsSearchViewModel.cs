namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportsSearchViewModel
    {
        public ReportsSearchViewModel(
            SelectList departments,
            ReportsSearchFilter filter, 
            int inventoryTypeId)
        {
            this.Departments = departments;
            this.Filter = filter;
            this.InventoryTypeId = inventoryTypeId;
        }

        [NotNull]
        public ReportsSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public int InventoryTypeId { get; private set; }

        public static ReportsSearchViewModel BuildViewModel(
            ReportsSearchFilter currentFilter,
            List<ItemOverview> departments,
            int inventoryTypeId)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new ReportsSearchViewModel(departmentsSelectList, currentFilter, inventoryTypeId);
        }
    }
}