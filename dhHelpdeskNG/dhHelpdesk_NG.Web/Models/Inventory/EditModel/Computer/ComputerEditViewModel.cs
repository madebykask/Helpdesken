namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;

    public class ComputerEditViewModel
    {
        public ComputerEditViewModel(
            ComputerViewModel computerViewModel,
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<ComputerLogOverview> computerLogs,
            List<InventoryGridModel> inventoryGridModels)
        {
            this.ComputerViewModel = computerViewModel;
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
            this.InventoryGridModels = inventoryGridModels;
        }

        public ComputerViewModel ComputerViewModel { get; set; }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }

        public List<InventoryGridModel> InventoryGridModels { get; private set; } 
    }
}