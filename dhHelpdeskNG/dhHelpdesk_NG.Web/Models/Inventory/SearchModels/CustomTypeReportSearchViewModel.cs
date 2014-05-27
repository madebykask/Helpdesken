namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CustomTypeReportSearchViewModel
    {
        public CustomTypeReportSearchViewModel(
            SelectList departments,
            CustomTypeReportsSearchFilter filter)
        {
            this.Departments = departments;
            this.Filter = filter;
        }

        [NotNull]
        public CustomTypeReportsSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public static CustomTypeReportSearchViewModel BuildViewModel(
            CustomTypeReportsSearchFilter currentFilter,
            List<ItemOverview> departments)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new CustomTypeReportSearchViewModel(departmentsSelectList, currentFilter);
        }
    }
}