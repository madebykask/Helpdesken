namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerRepository : Repository, IServerRepository
    {
        public ServerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
