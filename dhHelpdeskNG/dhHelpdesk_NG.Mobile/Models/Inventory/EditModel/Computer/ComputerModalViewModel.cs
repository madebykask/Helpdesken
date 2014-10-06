namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;

    public class ComputerModalViewModel
    {
        public ComputerModalViewModel(
            ComputerShortOverview computerShortOverview,
            ComputerFieldsSettingsOverviewForShortInfo settings,
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<ComputerLogOverview> computerLogs)
        {
            this.ComputerShortOverview = computerShortOverview;
            this.Settings = settings;
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
        }

        public ComputerShortOverview ComputerShortOverview { get; set; }

        public ComputerFieldsSettingsOverviewForShortInfo Settings { get; set; }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }
    }
}