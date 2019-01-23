namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventorySearchViewModel : BaseIndexModel
    {
        public InventorySearchViewModel(
            SelectList departments,
            InventorySearchFilter filter,
            InventoryFieldsSettingsOverviewForFilter settings,
            int inventoryTypeId,
            int currentMode,
            List<ItemOverview> types)
            : base(currentMode, types)
        {
            this.Filter = filter;
            this.Departments = departments;
            this.Settings = settings;
            this.InventoryTypeId = inventoryTypeId;
        }

        [NotNull]
        public InventorySearchFilter Filter { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [NotNull]
        public InventoryFieldsSettingsOverviewForFilter Settings { get; private set; }

        public string Name { get; private set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public static InventorySearchViewModel BuildViewModel(
            InventorySearchFilter currentFilter,
            List<ItemOverview> departments,
            InventoryFieldsSettingsOverviewForFilter settings,
            int inventoryTypeId,
            int currentMode,
            List<ItemOverview> types)
        {
            var list = new SelectList(departments, "Value", "Name");

            var inventoryType =
                types.FirstOrDefault(x => x.Value == inventoryTypeId.ToString(CultureInfo.InvariantCulture));

            return new InventorySearchViewModel(list, currentFilter, settings, inventoryTypeId, currentMode, types)
            {
                Name = inventoryType?.Name
            };
        }
    }
}