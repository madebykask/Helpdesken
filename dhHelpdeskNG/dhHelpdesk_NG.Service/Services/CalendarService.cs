// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ICalendarService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Calendars;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Calendars;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    /// <summary>
    /// The CalendarService interface.
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// The get calendars.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IList<CalendarOverview> GetCalendars(int customerId, bool secure = false, bool calendarWGRestriction = false);

        /// <summary>
        /// The search and generate calendar.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="searchCalendars">
        /// The search calendars.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IList<CalendarOverview> SearchAndGenerateCalendar(int customerId, ICalendarSearch searchCalendars, bool secure = false, bool calendarWGRestriction = false);

        /// <summary>
        /// The get calendar.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Calendar"/>.
        /// </returns>
        CalendarOverview GetCalendar(int id);

        /// <summary>
        /// The delete calendar.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DeleteMessage"/>.
        /// </returns>
        DeleteMessage DeleteCalendar(int id);

        /// <summary>
        /// The save calendar.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <param name="workGroups">
        /// The workGroups.
        /// </param>
        void SaveCalendar(CalendarOverview calendar, int[] workGroups);

        /// <summary>
        /// The commit.
        /// </summary>
        void Commit();

        IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers, int? count, bool forStartPage, bool untilTodayOnly, bool secure = false, bool calendarWGRestriction = false);
    }

    /// <summary>
    /// The calendar service.
    /// </summary>
    public class CalendarService : ICalendarService
    {
        /// <summary>
        /// The calendar repository.
        /// </summary>
        private readonly ICalendarRepository calendarRepository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The working group repository.
        /// </summary>
        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IWorkContext workContext;

        public CalendarService(
            ICalendarRepository calendarRepository,
            IUnitOfWork unitOfwork,
            IWorkingGroupRepository workingGroupRepository, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IWorkContext workContext)
        {
            this.calendarRepository = calendarRepository;
            this.unitOfWork = unitOfwork;
            this.workingGroupRepository = workingGroupRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.workContext = workContext;
        }

        /// <summary>
        /// The get calendars.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<CalendarOverview> GetCalendars(int customerId, bool secure = false, bool calendarWGRestriction = false)
        {
            return this.GetCalendarOverviews(new[] { customerId }, null, false, false, secure, calendarWGRestriction).ToList();
        }

        /// <summary>
        /// The search and generate calendar.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="searchCalendars">
        /// The search calendars.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<CalendarOverview> SearchAndGenerateCalendar(int customerId, ICalendarSearch searchCalendars, bool secure = false, bool calendarWGRestriction = false)
        {
            var query = from c in this.GetCalendarOverviews(new[] { customerId }, null, false, false, secure, calendarWGRestriction) select c;

            if (!string.IsNullOrEmpty(searchCalendars.SearchCs))
            {
                query = query.Where(x => x.Caption.ContainsText(searchCalendars.SearchCs)
                    || x.Text.ContainsText(searchCalendars.SearchCs));
            }

            if (!string.IsNullOrEmpty(searchCalendars.SortBy) && (searchCalendars.SortBy != "undefined"))
            {
                if (searchCalendars.Ascending)
                {
                    query = query.OrderBy(x => x.GetType().GetProperty(searchCalendars.SortBy).GetValue(x, null));
                }
                else
                {
                    query = query.OrderByDescending(x => x.GetType().GetProperty(searchCalendars.SortBy).GetValue(x, null));
                }
            }

            return query.ToList();
        }

        /// <summary>
        /// The get calendar.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Calendar"/>.
        /// </returns>
        public CalendarOverview GetCalendar(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<Calendar>();

                var entity = rep.GetAll()
                        .GetById(id)
                        .IncludePath(o => o.WGs)
                        .SingleOrDefault();

                return CalendarMapper.MapToOverview(entity);
            }
        }

        /// <summary>
        /// The delete calendar.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DeleteMessage"/>.
        /// </returns>
        public DeleteMessage DeleteCalendar(int id)
        {
            try
            {
                using (var uow = this.unitOfWorkFactory.Create())
                {
                    var rep = uow.GetRepository<Calendar>();

                    var entity = rep.GetAll()
                                  .GetById(id)
                                  .SingleOrDefault();

                    if (entity == null)
                    {
                        return DeleteMessage.Error;
                    }

                    entity.WGs.Clear();

                    rep.DeleteById(id);

                    uow.Save();
                    return DeleteMessage.Success;
                }
            }
            catch
            {
                return DeleteMessage.UnExpectedError;
            }
        }

        /// <summary>
        /// The save calendar.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <param name="workGroups">
        /// The work groups.
        /// </param>
        public void SaveCalendar(CalendarOverview calendar, int[] workGroups)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException("calendar");
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var calendarRep = uow.GetRepository<Calendar>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                Calendar entity;
                int userId = this.workContext.User.UserId;
                var now = DateTime.Now;
                if (calendar.Id == 0)
                {
                    entity = new Calendar();
                    CalendarMapper.MapToEntity(calendar, entity);
                    entity.CreatedDate = now;
                    entity.ChangedDate = now;
                    entity.ChangedByUser_Id = userId;
                    calendarRep.Add(entity);
                }
                else
                {
                    entity = calendarRep.GetById(calendar.Id);
                    CalendarMapper.MapToEntity(calendar, entity);
                    entity.ChangedDate = now;
                    entity.ChangedByUser_Id = userId;
                    calendarRep.Update(entity);
                }

                entity.WGs.Clear();
                if (workGroups != null)
                {
                    foreach (var wg in workGroups)
                    {
                        var workingGroupEntity = workingGroupRep.GetById(wg);
                        entity.WGs.Add(workingGroupEntity);
                    }
                }

                uow.Save();
            }                        
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            this.unitOfWork.Commit();
        }

        public IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers, int? count, bool forStartPage, bool untilTodayOnly, bool secure = false, bool calendarWGRestriction = false)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Calendar>();

                var query = repository.GetAll();

                if (untilTodayOnly)
                {
                    query = query
                        .GetFromDate()
                        .GetUntilDate();
                }

                if (secure)
                {

                    if (!calendarWGRestriction)
                        query = query.RestrictByWorkingGroups(this.workContext);
                    else
                        query = query.RestrictByWorkingGroupsOnlyRead(this.workContext);
                }
                
                return query.GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews().OrderBy(x => x.CalendarDate).ThenBy(x => x.CustomerName);
            }
        }
    }
}
