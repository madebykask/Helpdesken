using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
