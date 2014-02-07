namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
