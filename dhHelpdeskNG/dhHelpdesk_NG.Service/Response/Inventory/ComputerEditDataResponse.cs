namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;

    public class ComputerEditDataResponse
    {
        public ComputerEditDataResponse(
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<ComputerLogOverview> computerLogs)
        {
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.ComputerLogs = computerLogs;
        }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<ComputerLogOverview> ComputerLogs { get; private set; }
    }
}
