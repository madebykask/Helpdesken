using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region HOLIDAY

    public interface IHolidayRepository : IRepository<Holiday>
    {
    }

    public class HolidayRepository : RepositoryBase<Holiday>, IHolidayRepository
    {
        public HolidayRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region HOLIDAYHEADER

    public interface IHolidayHeaderRepository : IRepository<HolidayHeader>
    {
    }

    public class HolidayHeaderRepository : RepositoryBase<HolidayHeader>, IHolidayHeaderRepository
    {
        public HolidayHeaderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
