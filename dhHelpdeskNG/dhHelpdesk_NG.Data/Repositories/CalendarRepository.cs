using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
    }

    public class CalendarRepository : RepositoryBase<Calendar>, ICalendarRepository
    {
        public CalendarRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
