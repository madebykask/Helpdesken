namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;

    public class ServerEditViewModel
    {
        public ServerEditViewModel(
            ServerViewModel serverViewModel,
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<OperationServerLogOverview> operaionLogs)
        {
            this.ServerViewModel = serverViewModel;
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.OperaionLogs = operaionLogs;
        }

        public ServerViewModel ServerViewModel { get; set; }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<OperationServerLogOverview> OperaionLogs { get; private set; }
    }
}