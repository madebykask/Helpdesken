namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerEditViewModel
    {
        public ComputerEditViewModel(
            ComputerViewModel computerViewModel,
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<ComputerLogOverview> computerLogs,
            string activeTab)
        {
            this.ComputerViewModel = computerViewModel;
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
            this.ActiveTab = activeTab;
        }

        public ComputerViewModel ComputerViewModel { get; set; }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }

        [NotNullAndEmpty]
        public string ActiveTab { get; private set; }
    }
}