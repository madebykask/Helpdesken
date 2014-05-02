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
    using DH.Helpdesk.Dal.Mappers;
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

        /// <summary>
        /// The save calendar.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        void SaveCalendar(CalendarOverview calendar);

        /// <summary>
        /// The get calendar.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CalendarOverview"/>.
        /// </returns>
        CalendarOverview GetCalendar(int id);
    }

    /// <summary>
    /// The calendar repository.
    /// </summary>
    public class CalendarRepository : RepositoryBase<Calendar>, ICalendarRepository
    {
        /// <summary>
        /// The to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<Calendar, CalendarOverview> toBusinessModelMapper;

        /// <summary>
        /// The to entity mapper.
        /// </summary>
        private readonly IBusinessModelToEntityMapper<CalendarOverview, Calendar> toEntityMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        /// <param name="toBusinessModelMapper">
        /// The to Business Model Mapper.
        /// </param>
        /// <param name="toEntityMapper">
        /// The to Entity Mapper.
        /// </param>
        public CalendarRepository(
            IDatabaseFactory databaseFactory, 
            IWorkContext workContext, 
            IEntityToBusinessModelMapper<Calendar, CalendarOverview> toBusinessModelMapper, 
            IBusinessModelToEntityMapper<CalendarOverview, Calendar> toEntityMapper)
            : base(databaseFactory, workContext)
        {
            this.toBusinessModelMapper = toBusinessModelMapper;
            this.toEntityMapper = toEntityMapper;
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
            var entities = this.GetSecuredEntities(this.Table
                .Where(c => customers.Contains(c.Customer_Id))
                .Select(c => new
                {
                    c.Id,
                    c.Customer_Id,
                    c.CalendarDate,
                    c.Caption,
                    c.Text,
                    c.ShowOnStartPage,
                    c.ShowUntilDate,
                    c.PublicInformation,
                    c.ChangedByUser_Id,
                    c.ChangedDate,
                    c.CreatedDate,
                    c.WGs                            
                })
                .OrderByDescending(p => p.CalendarDate)
                .ToList()
                .Select(c => new Calendar
                {
                    Id = c.Id,
                    Customer_Id = c.Customer_Id,
                    CalendarDate = c.CalendarDate,
                    Caption = c.Caption,
                    Text = c.Text,
                    ShowOnStartPage = c.ShowOnStartPage,
                    ShowUntilDate = c.ShowUntilDate,
                    PublicInformation = c.PublicInformation,
                    ChangedByUser_Id = c.ChangedByUser_Id,
                    ChangedDate = c.ChangedDate,
                    CreatedDate = c.CreatedDate,
                    WGs = c.WGs
                }));

            return entities.Select(c => new CalendarOverview
                {
                    Id = c.Id,
                    CustomerId = c.Customer_Id,
                    CalendarDate = c.CalendarDate,
                    Caption = c.Caption,
                    Text = c.Text,
                    ShowOnStartPage = c.ShowOnStartPage.ToBool(),
                    ShowUntilDate = c.ShowUntilDate,
                    PublicInformation = c.PublicInformation.ToBool(),
                    ChangedByUserId = c.ChangedByUser_Id,
                    ChangedDate = c.ChangedDate,
                    CreatedDate = c.CreatedDate,
                    WGs = c.WGs
                });
        }

        /// <summary>
        /// The save calendar.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        public void SaveCalendar(CalendarOverview calendar)
        {
            var entity = new Calendar();
            this.toEntityMapper.Map(calendar, entity);

            if (entity.IsNew())
            {
                this.Add(entity);
                this.Commit();
                return;
            }

            this.Update(entity);
            this.Commit();            
        }

        /// <summary>
        /// The get calendar.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CalendarOverview"/>.
        /// </returns>
        public CalendarOverview GetCalendar(int id)
        {
            var entity = this.GetById(id);

            return this.toBusinessModelMapper.Map(entity);
        }
    }
}
