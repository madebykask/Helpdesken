namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;

    public class ComputerEditDataResponse
    {
        public ComputerEditDataResponse(
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<ComputerLogOverview> computerLogs,
            InventoryOverviewResponseWithType inventoryOverviewResponseWithType, 
            InventoriesFieldSettingsOverviewResponse inventoriesFieldSettingsOverviewResponse, 
            List<ItemOverview> inventoryTypes)
        {
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
            this.InventoryOverviewResponseWithType = inventoryOverviewResponseWithType;
            this.InventoriesFieldSettingsOverviewResponse = inventoriesFieldSettingsOverviewResponse;
            this.InventoryTypes = inventoryTypes;
        }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }

        public InventoryOverviewResponseWithType InventoryOverviewResponseWithType { get; private set; }

        public InventoriesFieldSettingsOverviewResponse InventoriesFieldSettingsOverviewResponse { get; private set; }

        public List<ItemOverview> InventoryTypes { get; private set; }
    }
}
