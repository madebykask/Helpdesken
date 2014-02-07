namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
            this._watchDateCalendarRepository = watchDateCalendarRepository;
            this._watchDateCalendarValueRepository = watchDateCalendarValueRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<WatchDateCalendar> GetAllWatchDateCalendars()
        {
            return this._watchDateCalendarRepository.GetAll();
        }

        public IEnumerable<WatchDateCalendarValue> GetAllWatchDateCalendarValues()
        {
            return this._watchDateCalendarValueRepository.GetAll().OrderByDescending(x => x.WatchDate);
        }

        public WatchDateCalendar GetWatchDateCalendar(int id)
        {
            return this._watchDateCalendarRepository.GetById(id);
        }

        public WatchDateCalendarValue GetWatchDateCalendarValue(int id)
        {
            return this._watchDateCalendarValueRepository.GetById(id);
        }

        public void SaveWatchDateCalendarValue(WatchDateCalendarValue watchDateCalendarValue, out IDictionary<string, string> errors)
        {
            if (watchDateCalendarValue == null)
                throw new ArgumentNullException("watchDateCalendarValue");

            errors = new Dictionary<string, string>();

            if (watchDateCalendarValue.Id == 0)
                this._watchDateCalendarValueRepository.Add(watchDateCalendarValue);
            else
                this._watchDateCalendarValueRepository.Update(watchDateCalendarValue);

            if (watchDateCalendarValue.WatchDateCalendar.Id == 0)
            {
                watchDateCalendarValue.WatchDateCalendar.CreatedDate = DateTime.Now;
                this._watchDateCalendarRepository.Add(watchDateCalendarValue.WatchDateCalendar);
            }
            else
                this._watchDateCalendarRepository.Update(watchDateCalendarValue.WatchDateCalendar);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
