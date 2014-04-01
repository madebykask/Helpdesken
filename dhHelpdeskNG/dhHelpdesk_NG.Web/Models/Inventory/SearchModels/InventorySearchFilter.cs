namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public class InventorySearchFilter
    {
        public InventorySearchFilter(int customerId, ConfigurableSearchFieldModel<List<SelectListItem>> departments, int? departmentId, string searchFor, int recordsOnPage)
        {
            this.CustomerId = customerId;
            this.Departments = departments;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
            this.RecordsOnPage = recordsOnPage;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Departments { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; private set; }
    }
}