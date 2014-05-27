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
            ReportsSearchFilter filter)
        {
            this.Departments = departments;
            this.Filter = filter;
        }

        [NotNull]
        public ReportsSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public static ReportsSearchViewModel BuildViewModel(
            ReportsSearchFilter currentFilter,
            List<ItemOverview> departments)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new ReportsSearchViewModel(departmentsSelectList, currentFilter);
        }
    }
}