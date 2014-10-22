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
        IList<CalendarOverview> GetCalendars(int customerId);

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
        IList<CalendarOverview> SearchAndGenerateCalendar(int customerId, ICalendarSearch searchCalendars);

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

        /// <summary>
        /// The get calendar overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers, int? count, bool forStartPage);
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
        public IList<CalendarOverview> GetCalendars(int customerId)
        {
            return this.calendarRepository.GetCalendarOverviews(new[] { customerId }).ToList();
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
        public IList<CalendarOverview> SearchAndGenerateCalendar(int customerId, ICalendarSearch searchCalendars)
        {
            var query = from c in this.calendarRepository.GetCalendarOverviews(new[] { customerId }) select c;

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
            return this.calendarRepository.GetCalendar(id);
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
            var calendar = this.calendarRepository.GetById(id);

            if (calendar != null)
            {
                try
                {
                    this.calendarRepository.Delete(calendar);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
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

            if (calendar.WGs != null)
            {
                foreach (var delete in calendar.WGs.ToList())
                {
                    calendar.WGs.Remove(delete);
                }
            }
            else
            {
                calendar.WGs = new List<WorkingGroupEntity>();
            }

            if (workGroups != null)
            {
                foreach (int id in workGroups)
                {
                    var wg = this.workingGroupRepository.GetById(id);

                    if (wg != null)
                    {
                        calendar.WGs.Add(wg);
                    }
                }
            }
            
            this.calendarRepository.SaveCalendar(calendar);
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// The get calendar overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Calendar>();

                return repository.GetAll()
//                        .GetUntilToday()
                        .RestrictByWorkingGroups(this.workContext)
                        .GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }
    }
}
