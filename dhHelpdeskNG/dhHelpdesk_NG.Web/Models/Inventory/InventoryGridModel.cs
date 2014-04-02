namespace DH.Helpdesk.Web.Models.Inventory
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class InventoryGridModel
    {
        public InventoryGridModel(
            int inventoriesFound,
            List<GridColumnHeaderModel> headers,
            List<InventoryOverviewModel> inventories)
        {
            this.InventoriesFound = inventoriesFound;
            this.Headers = headers;
            this.Inventories = inventories;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<InventoryOverviewModel> Inventories { get; set; }

        [MinValue(0)]
        public int InventoriesFound { get; set; }

        public CurrentModes CurrentMode { get; set; }

        public static InventoryGridModel BuildModel(List<ComputerOverview> modelList, ComputerFieldsSettingsOverview settings)
        {
            throw new NotImplementedException();
        }

        public static InventoryGridModel BuildModel(List<ServerOverview> modelList, ServerFieldsSettingsOverview settings)
        {
            throw new NotImplementedException();
        }

        public static InventoryGridModel BuildModel(List<PrinterOverview> modelList, PrinterFieldsSettingsOverview settings)
        {
            throw new NotImplementedException();
        }

        public static InventoryGridModel BuildModel(InventoryOverviewResponse modelList, InventoryFieldSettingsOverviewResponse settings)
        {
            throw new NotImplementedException();
        }
    }
}