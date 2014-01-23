using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Linq;

    #region TIMETYPE

    public interface ITimeTypeRepository : IRepository<TimeType>
    {
    }

    public class TimeTypeRepository : RepositoryBase<TimeType>, ITimeTypeRepository
    {
        public TimeTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region TIMEREGISTRATION

    public interface ITimeRegistrationRepository : IRepository<TimeRegistration>
    {
    }

    public class TimeRegistrationRepository : RepositoryBase<TimeRegistration>, ITimeRegistrationRepository
    {
        public TimeRegistrationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
