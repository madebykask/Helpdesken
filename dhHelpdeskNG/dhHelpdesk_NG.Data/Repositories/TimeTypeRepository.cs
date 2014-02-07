namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
