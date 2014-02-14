namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class LogicalDriveRepository : Repository, ILogicalDriveRepository
    {
        public LogicalDriveRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
