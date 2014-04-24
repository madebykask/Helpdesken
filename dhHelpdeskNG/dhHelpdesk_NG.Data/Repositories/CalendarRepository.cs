// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ICalendarRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The CalendarRepository interface.
    /// </summary>
    public interface ICalendarRepository : IRepository<Calendar>
    {
        /// <summary>
        /// The get calendar overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers);
    }

    /// <summary>
    /// The calendar repository.
    /// </summary>
    public class CalendarRepository : RepositoryBase<Calendar>, ICalendarRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        public CalendarRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
        }

        /// <summary>
        /// The get calendar overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers)
        {
            return GetSecuredEntities()
                .Where(c => customers.Contains(c.Customer_Id))
                .Select(c => new CalendarOverview()
                {
                    CustomerId = c.Customer_Id,
                    CalendarDate = c.CalendarDate,
                    Caption = c.Caption,
                    Text = c.Text,
                    ShowOnStartPage = c.ShowOnStartPage.ToBool(),
                    ShowUntilDate = c.ShowUntilDate
                })
                .OrderByDescending(p => p.CalendarDate)
                .ToList(); 
        }
    }
}
