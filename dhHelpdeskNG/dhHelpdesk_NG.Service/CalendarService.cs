using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICalendarService
    {
        IList<Calendar> GetCalendars(int customerId);
        IList<Calendar> SearchAndGenerateCalendar(int customerId, ICalendarSearch SearchCalendars);

        Calendar GetCalendar(int id);

        DeleteMessage DeleteCalendar(int id);

        void SaveCalendar(Calendar calendar, int[] wgs, out IDictionary<string, string> errors);
        void Commit();
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
            _calendarRepository = calendarRepository;
            _unitOfWork = unitOfwork;
            _workingGroupRepository = workingGroupRepository;
        }

        public IList<Calendar> GetCalendars(int customerId)
        {
            return _calendarRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<Calendar> SearchAndGenerateCalendar(int customerId, ICalendarSearch SearchCalendars)
        {
            var query = (from c in _calendarRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select c);

            if (!string.IsNullOrEmpty(SearchCalendars.SearchCs))
                query = query.Where(x => x.Caption.Contains(SearchCalendars.SearchCs)
                    || x.Text.Contains(SearchCalendars.SearchCs));

            return query.OrderBy(x => x.ChangedDate).ToList();
        }

        public Calendar GetCalendar(int id)
        {
            return _calendarRepository.GetById(id);
        }

        public DeleteMessage DeleteCalendar(int id)
        {
            var calendar = _calendarRepository.GetById(id);

            if (calendar != null)
            {
                try
                {
                    _calendarRepository.Delete(calendar);
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
                calendar.WGs = new List<WorkingGroup>();

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = _workingGroupRepository.GetById(id);

                    if (wg != null)
                        calendar.WGs.Add(wg);
                }
            }

            if (calendar.Id == 0)
                _calendarRepository.Add(calendar);
            else
                _calendarRepository.Update(calendar);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
