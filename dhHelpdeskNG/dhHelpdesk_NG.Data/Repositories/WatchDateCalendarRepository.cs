namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region WATCHDATECALENDAR

    public interface IWatchDateCalendarRepository : IRepository<WatchDateCalendar>
    {
    }

    public class WatchDateCalendarRepository : RepositoryBase<WatchDateCalendar>, IWatchDateCalendarRepository
    {
        public WatchDateCalendarRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region WATCHDATECALENDARVALUE

    public interface IWatchDateCalendarValueRepository : IRepository<WatchDateCalendarValue>
    {
    }

    public class WatchDateCalendarValueRepository : RepositoryBase<WatchDateCalendarValue>, IWatchDateCalendarValueRepository
    {
        public WatchDateCalendarValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
