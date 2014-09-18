namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
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
        IEnumerable<WatchDateCalendarValue> GetWDCalendarValuesByWDCId(int id);
    }

    public class WatchDateCalendarValueRepository : RepositoryBase<WatchDateCalendarValue>, IWatchDateCalendarValueRepository
    {
        public WatchDateCalendarValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<WatchDateCalendarValue> GetWDCalendarValuesByWDCId(int id)
        {
            return (from wd in this.DataContext.WatchDateCalendarValues
                    where wd.WatchDateCalendar_Id == id
                    select wd).ToList();
        }
    }

    #endregion
}
