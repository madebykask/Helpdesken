namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerLogicalDriveRepository : Repository, IServerLogicalDriveRepository
    {
        public ServerLogicalDriveRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<LogicalDriveOverview> Find(int serverId)
        {
            var anonymus = this.DbContext.ServerLogicalDrives.Where(x => x.Server_Id == serverId).Select(c => new { c.Id, c.Server_Id, c.DriveLetter, c.FileSystemName, c.FreeBytes, c.TotalBytes }).ToList();

            var overviews = anonymus.Select(c => new LogicalDriveOverview(c.Id, c.Server_Id, c.FreeBytes, c.TotalBytes, c.DriveLetter, c.FileSystemName)).ToList();

            return overviews;
        }
    }
}