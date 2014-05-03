namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

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
            InventoryOverviewResponse inventoryOverviewResponse)
        {
            this.ComputerEditAggregate = computerEditAggregate;
            this.Settings = settings;
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
            this.InventoryOverviewResponse = inventoryOverviewResponse;
        }

        public ComputerEditAggregate ComputerEditAggregate { get; private set; }

        public ComputerFieldsSettingsForModelEdit Settings { get; private set; }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }

        public InventoryOverviewResponse InventoryOverviewResponse { get; private set; }
    }
}
