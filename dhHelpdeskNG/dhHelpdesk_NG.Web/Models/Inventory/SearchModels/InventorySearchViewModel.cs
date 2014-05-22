namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventorySearchViewModel
    {
        public InventorySearchViewModel(
            SelectList departments,
            InventorySearchFilter filter,
            InventoryFieldsSettingsOverviewForFilter settings)
        {
            this.Filter = filter;
            this.Departments = departments;
            this.Settings = settings;
        }

        [NotNull]
        public InventorySearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        [NotNull]
        public InventoryFieldsSettingsOverviewForFilter Settings { get; private set; }

        public static InventorySearchViewModel BuildViewModel(
            InventorySearchFilter currentFilter,
            List<ItemOverview> departments,
            InventoryFieldsSettingsOverviewForFilter settings)
        {
            var list = new SelectList(departments, "Value", "Name");

            return new InventorySearchViewModel(list, currentFilter, settings);
        }
    }
}