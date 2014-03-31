using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Calendar.Output;
using DH.Helpdesk.BusinessData.Models.Common.Output;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICalendarRepository : IRepository<Calendar>
    {
        IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers);
    }

    public class CalendarRepository : RepositoryBase<Calendar>, ICalendarRepository
    {
        public CalendarRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers)
        {
            return DataContext.Calendars
                .Where(c => customers.Contains(c.Customer_Id))
                .Select(c => new CalendarOverview()
                {
                    Customer_Id = c.Customer_Id,
                    CalendarDate = c.CalendarDate,
                    Caption = c.Caption,
                    Text = c.Text
                });
        }
    }
}
