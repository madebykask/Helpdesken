namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region PERMISSION

    public interface IPermissionRepository : IRepository<Permission>
    {
    }

    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PERMISSIONLANGUAGE

    public interface IPermissionLanguageRepository : IRepository<PermissionLanguage>
    {
    }

    public class PermissionLanguageRepository : RepositoryBase<PermissionLanguage>, IPermissionLanguageRepository
    {
        public PermissionLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
