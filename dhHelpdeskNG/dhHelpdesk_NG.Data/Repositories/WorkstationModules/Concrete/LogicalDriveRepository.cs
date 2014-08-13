namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class LogicalDriveRepository : Repository, ILogicalDriveRepository
    {
        public LogicalDriveRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<LogicalDriveOverview> Find(int computerId)
        {
            var anonymus = this.DbContext.LogicalDrives.Where(x => x.Computer_Id == computerId).Select(c => new { c.Id, c.Computer_Id, c.DriveLetter, c.FileSystemName, c.FreeBytes, c.TotalBytes }).ToList();

            var overviews = anonymus.Select(c => new LogicalDriveOverview(c.Id, c.Computer_Id, c.FreeBytes, c.TotalBytes, c.DriveLetter, c.FileSystemName)).ToList();

            return overviews;
        }

        public void DeleteByComputerId(int computerId)
        {
            var entities = this.DbContext.LogicalDrives.Where(x => x.Computer_Id == computerId).ToList();
            entities.ForEach(x => this.DbContext.LogicalDrives.Remove(x));
        }
    }
}
