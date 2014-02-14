namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerLogicalDriveRepository : Repository, IServerLogicalDriveRepository
    {
        public ServerLogicalDriveRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}