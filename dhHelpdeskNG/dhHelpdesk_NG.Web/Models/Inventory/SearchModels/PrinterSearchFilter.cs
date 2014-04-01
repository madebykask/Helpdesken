namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public class PrinterSearchFilter
    {
        public PrinterSearchFilter(int customerId, ConfigurableSearchFieldModel<List<SelectListItem>> departments, int? departmentId, string searchFor)
        {
            this.CustomerId = customerId;
            this.Departments = departments;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Departments { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }
    }
}