namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;

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
            CustomTypeFiltersResponse additionalData,
            InventoryFieldsSettingsOverviewForFilter settings)
        {
            var departments = new SelectList(additionalData.Departments, "Value", "Name");

            return new InventorySearchViewModel(departments, currentFilter, settings);
        }
    }
}