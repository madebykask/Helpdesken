namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerFieldSettingsRepository : Repository, IServerFieldSettingsRepository
    {
        public ServerFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}