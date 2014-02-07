namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region SERVER

    public interface IServerRepository : IRepository<Server>
    {
    }

    public class ServerRepository : RepositoryBase<Server>, IServerRepository
    {
        public ServerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region SERVERFIELDSETTINGS

    public interface IServerFieldSettingsRepository : IRepository<ServerFieldSettings>
    {
    }

    public class ServerFieldSettingsRepository : RepositoryBase<ServerFieldSettings>, IServerFieldSettingsRepository
    {
        public ServerFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region SERVERLOGICALDRIVE

    public interface IServerLogicalDriveRepository : IRepository<ServerLogicalDrive>
    {
    }

    public class ServerLogicalDriveRepository : RepositoryBase<ServerLogicalDrive>, IServerLogicalDriveRepository
    {
        public ServerLogicalDriveRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region SERVERSOFTWARE

    public interface IServerSoftwareRepository : IRepository<ServerSoftware>
    {
    }

    public class ServerSoftwareRepository : RepositoryBase<ServerSoftware>, IServerSoftwareRepository
    {
        public ServerSoftwareRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
