namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InstaledProgramsReportSearchViewModel
    {
        public InstaledProgramsReportSearchViewModel(
            SelectList departments,
            InstaledProgramsReportSearchFilter filter)
        {
            this.Departments = departments;
            this.Filter = filter;
        }

        [NotNull]
        public InstaledProgramsReportSearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        public static InstaledProgramsReportSearchViewModel BuildViewModel(
            InstaledProgramsReportSearchFilter currentFilter,
            List<ItemOverview> departments)
        {
            var departmentsSelectList = new SelectList(departments, "Value", "Name");

            return new InstaledProgramsReportSearchViewModel(departmentsSelectList, currentFilter);
        }
    }
}