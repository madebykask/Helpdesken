namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerSoftwareRepository : Repository, IServerSoftwareRepository
    {
        public ServerSoftwareRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}