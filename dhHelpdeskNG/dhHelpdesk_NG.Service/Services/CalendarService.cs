using DH.Helpdesk.BusinessData.Models.Calendar.Output;
using DH.Helpdesk.BusinessData.Models.Common.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICalendarService
    {
        IList<Calendar> GetCalendars(int customerId);
        IList<Calendar> SearchAndGenerateCalendar(int customerId, ICalendarSearch SearchCalendars);

        Calendar GetCalendar(int id);

        DeleteMessage DeleteCalendar(int id);

        void SaveCalendar(Calendar calendar, int[] wgs, out IDictionary<string, string> errors);
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
        IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers, int? count = null, bool forStartPage = true);
    }

    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        public CalendarService(
            ICalendarRepository calendarRepository,
            IUnitOfWork unitOfwork,
            IWorkingGroupRepository workingGroupRepository)
        {
            this._calendarRepository = calendarRepository;
            this._unitOfWork = unitOfwork;
            this._workingGroupRepository = workingGroupRepository;
        }

        public IList<Calendar> GetCalendars(int customerId)
        {
            return this._calendarRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<Calendar> SearchAndGenerateCalendar(int customerId, ICalendarSearch SearchCalendars)
        {
            var query = (from c in this._calendarRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select c);

            if (!string.IsNullOrEmpty(SearchCalendars.SearchCs))
                query = query.Where(x => x.Caption.ContainsText(SearchCalendars.SearchCs)
                    || x.Text.ContainsText(SearchCalendars.SearchCs));

            if (!string.IsNullOrEmpty(SearchCalendars.SortBy) && (SearchCalendars.SortBy != "undefined"))
            {
                if (SearchCalendars.Ascending)
                    query = query.OrderBy(x => x.GetType().GetProperty(SearchCalendars.SortBy).GetValue(x, null));
                else
                    query = query.OrderByDescending(x => x.GetType().GetProperty(SearchCalendars.SortBy).GetValue(x, null));
            }

            return query.ToList();


        }

        public Calendar GetCalendar(int id)
        {
            return this._calendarRepository.GetById(id);
        }

        public DeleteMessage DeleteCalendar(int id)
        {
            var calendar = this._calendarRepository.GetById(id);

            if (calendar != null)
            {
                try
                {
                    this._calendarRepository.Delete(calendar);
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

        public void SaveCalendar(Calendar calendar, int[] wgs, out IDictionary<string, string> errors)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(calendar.Caption))
                errors.Add("Calendar.Caption", "Du måste ange en rubrik");

            if (string.IsNullOrEmpty(calendar.Text))
                errors.Add("Calendar.Text", "Du måste ange en text");

            if (calendar.WGs != null)
                foreach (var delete in calendar.WGs.ToList())
                    calendar.WGs.Remove(delete);
            else
                calendar.WGs = new List<WorkingGroupEntity>();

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = this._workingGroupRepository.GetById(id);

                    if (wg != null)
                        calendar.WGs.Add(wg);
                }
            }

            if (calendar.Id == 0)
                this._calendarRepository.Add(calendar);
            else
                this._calendarRepository.Update(calendar);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
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
        public IEnumerable<CalendarOverview> GetCalendarOverviews(int[] customers, int? count = null, bool forStartPage = true)
        {
            var today = DateTime.Today.RoundToDay();
            var calendars = this._calendarRepository.GetCalendarOverviews(customers)
                            .Where(c => c.ShowUntilDate.RoundToDay() >= today);
            if (forStartPage)
            {
                calendars = calendars.Where(c => c.ShowOnStartPage);
            }

            if (!count.HasValue)
            {
                return calendars;
            }

            return calendars.Take(count.Value);
        }
    }
}
