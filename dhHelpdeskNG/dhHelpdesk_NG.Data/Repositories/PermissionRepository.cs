using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
