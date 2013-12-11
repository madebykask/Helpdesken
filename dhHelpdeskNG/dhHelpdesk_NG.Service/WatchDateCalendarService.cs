using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IWatchDateCalendarService
    {
        IEnumerable<WatchDateCalendar> GetAllWatchDateCalendars();
        IEnumerable<WatchDateCalendarValue> GetAllWatchDateCalendarValues();

        WatchDateCalendar GetWatchDateCalendar(int id);
        WatchDateCalendarValue GetWatchDateCalendarValue(int id);

        void SaveWatchDateCalendarValue(WatchDateCalendarValue watchDateCalendarValue, out IDictionary<string, string> errors);
        void Commit();
    }

    public class WatchDateCalendarService : IWatchDateCalendarService
    {
        private readonly IWatchDateCalendarRepository _watchDateCalendarRepository;
        private readonly IWatchDateCalendarValueRepository _watchDateCalendarValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WatchDateCalendarService(
            IWatchDateCalendarRepository watchDateCalendarRepository,
            IWatchDateCalendarValueRepository watchDateCalendarValueRepository,
            IUnitOfWork unitOfWork)
        {
            _watchDateCalendarRepository = watchDateCalendarRepository;
            _watchDateCalendarValueRepository = watchDateCalendarValueRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<WatchDateCalendar> GetAllWatchDateCalendars()
        {
            return _watchDateCalendarRepository.GetAll();
        }

        public IEnumerable<WatchDateCalendarValue> GetAllWatchDateCalendarValues()
        {
            return _watchDateCalendarValueRepository.GetAll().OrderByDescending(x => x.WatchDate);
        }

        public WatchDateCalendar GetWatchDateCalendar(int id)
        {
            return _watchDateCalendarRepository.GetById(id);
        }

        public WatchDateCalendarValue GetWatchDateCalendarValue(int id)
        {
            return _watchDateCalendarValueRepository.GetById(id);
        }

        public void SaveWatchDateCalendarValue(WatchDateCalendarValue watchDateCalendarValue, out IDictionary<string, string> errors)
        {
            if (watchDateCalendarValue == null)
                throw new ArgumentNullException("watchDateCalendarValue");

            errors = new Dictionary<string, string>();

            if (watchDateCalendarValue.Id == 0)
                _watchDateCalendarValueRepository.Add(watchDateCalendarValue);
            else
                _watchDateCalendarValueRepository.Update(watchDateCalendarValue);

            if (watchDateCalendarValue.WatchDateCalendar.Id == 0)
            {
                watchDateCalendarValue.WatchDateCalendar.CreatedDate = DateTime.Now;
                _watchDateCalendarRepository.Add(watchDateCalendarValue.WatchDateCalendar);
            }
            else
                _watchDateCalendarRepository.Update(watchDateCalendarValue.WatchDateCalendar);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
