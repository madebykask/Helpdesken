namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;

    public class ServerEditDataResponse
    {
        public ServerEditDataResponse(
            List<SoftwareOverview> softwaries,
            List<LogicalDriveOverview> logicalDrives,
            List<OperationServerLogOverview> operationLogs)
        {
            this.Softwaries = softwaries;
            this.LogicalDrives = logicalDrives;
            this.OperationLogs = operationLogs;
        }

        public List<SoftwareOverview> Softwaries { get; private set; }

        public List<LogicalDriveOverview> LogicalDrives { get; private set; }

        public List<OperationServerLogOverview> OperationLogs { get; private set; }
    }
}