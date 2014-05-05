namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;

    public class ComputerEditResponse
    {
        public ComputerEditResponse(
            ComputerEditAggregate computerEditAggregate,
            ComputerFieldsSettingsForModelEdit settings,
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<ComputerLogOverview> computerLogs,
            InventoryOverviewResponseWithType inventoryOverviewResponseWithType, 
            InventoriesFieldSettingsOverviewResponse inventoriesFieldSettingsOverviewResponse, 
            List<ItemOverview> inventoryTypes)
        {
            this.ComputerEditAggregate = computerEditAggregate;
            this.Settings = settings;
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
            this.InventoryOverviewResponseWithType = inventoryOverviewResponseWithType;
            this.InventoriesFieldSettingsOverviewResponse = inventoriesFieldSettingsOverviewResponse;
            this.InventoryTypes = inventoryTypes;
        }

        public ComputerEditAggregate ComputerEditAggregate { get; private set; }

        public ComputerFieldsSettingsForModelEdit Settings { get; private set; }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }

        public InventoryOverviewResponseWithType InventoryOverviewResponseWithType { get; private set; }

        public InventoriesFieldSettingsOverviewResponse InventoriesFieldSettingsOverviewResponse { get; private set; }

        public List<ItemOverview> InventoryTypes { get; private set; }
    }
}
