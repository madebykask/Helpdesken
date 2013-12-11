using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region LICENSE

    public interface ILicenseRepository : IRepository<License>
    {
    }

    public class LicenseRepository : RepositoryBase<License>, ILicenseRepository
    {
        public LicenseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region LICENSEFILE

    public interface ILicenseFileRepository : IRepository<LicenseFile>
    {
    }

    public class LicenseFileRepository : RepositoryBase<LicenseFile>, ILicenseFileRepository
    {
        public LicenseFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
