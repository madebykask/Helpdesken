namespace DH.Helpdesk.Dal.Repositories.Servers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IServerLogicalDriveRepository : INewRepository
    {
        List<LogicalDriveOverview> Find(int serverId);
    }
}