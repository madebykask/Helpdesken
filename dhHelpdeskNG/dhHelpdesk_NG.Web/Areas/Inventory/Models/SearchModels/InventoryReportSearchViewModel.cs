namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryReportSearchViewModel : ReportsIndexViewModel
    {
        public InventoryReportSearchViewModel(
            SelectList departments,
            InventoryReportSearchFilter filter,
            int reportType,
            List<ItemOverview> inventoryTypes)
            : base(reportType, inventoryTypes)
        {
            this.Departments = departments;
            this.Filter = filter;
        }

        [NotNull]
        public InventoryReportSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public static InventoryReportSearchViewModel BuildViewModel(
            InventoryReportSearchFilter currentFilter,
            List<ItemOverview> departments,
            int reportType,
            List<ItemOverview> inventoryTypes)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");
            return new InventoryReportSearchViewModel(departmentsSelectList, currentFilter, reportType, inventoryTypes);
        }
    }
}