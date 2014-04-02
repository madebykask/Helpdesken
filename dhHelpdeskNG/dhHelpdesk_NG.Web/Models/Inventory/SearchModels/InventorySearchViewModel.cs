namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Common;

    public class InventorySearchViewModel
    {
        public InventorySearchViewModel(ConfigurableSearchFieldModel<List<SelectListItem>> departments, InventorySearchFilter filter)
        {
            this.Filter = filter;
            this.Departments = departments;
        }

        [NotNull]
        public InventorySearchFilter Filter { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Departments { get; private set; }

        public static InventorySearchViewModel BuildViewModel(InventorySearchFilter currentFilter, CustomTypeFiltersResponse filters, InventoryFieldSettingsOverviewResponse settings)
        {
            throw new System.NotImplementedException();
        }
    }
}