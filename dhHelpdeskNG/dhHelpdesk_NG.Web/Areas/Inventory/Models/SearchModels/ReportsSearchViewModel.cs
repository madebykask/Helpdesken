namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportsSearchViewModel : ReportsIndexViewModel
    {
        private ReportsSearchViewModel(
            int reportType,
            List<ItemOverview> reportTypes,
            ReportsSearchFilter filter,
            SelectList departments)
            : base(reportType, reportTypes)
        {
            this.Filter = filter;
            this.Departments = departments;
        }

        [NotNull]
        public ReportsSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public static ReportsSearchViewModel BuildViewModel(
            ReportsSearchFilter currentFilter,
            List<ItemOverview> departments,
            int reportType,
            List<ItemOverview> reportTypes)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new ReportsSearchViewModel(
                reportType,
                reportTypes,
                currentFilter,
                departmentsSelectList);
        }
    }
}