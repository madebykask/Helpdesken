namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CustomTypeReportSearchViewModel : ReportsIndexViewModel
    {
        public CustomTypeReportSearchViewModel(
            SelectList departments,
            CustomTypeReportsSearchFilter filter,
            int inventoryTypeId,
            int reportType,
            List<ItemOverview> overviews)
            : base(reportType, overviews)
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
            int inventoryTypeId,
            int reportType,
            List<ItemOverview> overviews)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new CustomTypeReportSearchViewModel(
                departmentsSelectList,
                currentFilter,
                inventoryTypeId,
                reportType,
                overviews);
        }
    }
}