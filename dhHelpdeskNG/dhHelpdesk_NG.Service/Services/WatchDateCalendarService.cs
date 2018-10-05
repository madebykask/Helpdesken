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
        IEnumerable<WatchDateCalendarValue> GetWDCalendarValuesByWDCId(int id);
        IEnumerable<WatchDateCalendarValue> GetWDCalendarValuesByWDCIdAndYear(int id, int year);

        IList<WatchDateCalendarValue> GetWDCalendarValuesByWDCIdAndYearForList(int id, int year);

        WatchDateCalendar GetWatchDateCalendar(int id);
        WatchDateCalendarValue GetWatchDateCalendarValue(int id);

        DeleteMessage DeleteWDCV(int id);

        void SaveWatchDateCalendar(WatchDateCalendar watchDateCalendar, out IDictionary<string, string> errors);
        void SaveWatchDateCalendarValue(WatchDateCalendarValue watchDateCalendarValue, out IDictionary<string, string> errors);

        DateTime? GetClosestDateTo(int calendarId, DateTime now);

        List<WatchDateCalendarValue> GetAllClosestDateTo(DateTime now);

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

        public IEnumerable<WatchDateCalendarValue> GetWDCalendarValuesByWDCId(int id)
        {
            return this._watchDateCalendarValueRepository.GetWDCalendarValuesByWDCId(id).OrderBy(x => x.WatchDate);
        }

        public IEnumerable<WatchDateCalendarValue> GetWDCalendarValuesByWDCIdAndYear(int id, int year)
        {
            return this._watchDateCalendarValueRepository.GetWDCalendarValuesByWDCIdAndYear(id, year).OrderBy(x => x.WatchDate);
        }

        public IList<WatchDateCalendarValue> GetWDCalendarValuesByWDCIdAndYearForList(int id, int year)
        {
            var query = (from cs in this._watchDateCalendarValueRepository.GetMany(x => x.WatchDateCalendar_Id == id && x.WatchDate.Year == year)
                         select cs);

            
            return query.OrderBy(x => x.WatchDate).ToList();
        }

        public WatchDateCalendar GetWatchDateCalendar(int id)
        {
            return this._watchDateCalendarRepository.GetById(id);
        }

        public WatchDateCalendarValue GetWatchDateCalendarValue(int id)
        {
            return this._watchDateCalendarValueRepository.GetById(id);
        }

        public void SaveWatchDateCalendar(WatchDateCalendar watchDateCalendar, out IDictionary<string, string> errors)
        {
            if (watchDateCalendar == null)
                throw new ArgumentNullException("watchDateCalendar");

            errors = new Dictionary<string, string>();

            if (watchDateCalendar.Id == 0)
                this._watchDateCalendarRepository.Add(watchDateCalendar);
            else
                this._watchDateCalendarRepository.Update(watchDateCalendar);


            if (errors.Count == 0)
                this.Commit();
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

        public DateTime? GetClosestDateTo(int calendarId, DateTime now)
        {
            
            var watchDateCalendarValue =
                this._watchDateCalendarValueRepository.GetAll()
                    .Where(it => it.WatchDateCalendar_Id == calendarId && it.WatchDate > now && (it.ValidUntilDate == null || it.ValidUntilDate.Value.Date >= now.Date))
                    .OrderBy(it => it.WatchDate)
                    .FirstOrDefault();
            if (watchDateCalendarValue != null)
            {
                return watchDateCalendarValue.WatchDate;
            }
            return null;
        }

        public List<WatchDateCalendarValue> GetAllClosestDateTo(DateTime now)
        {

            var watchDateCalendarValue =
                    _watchDateCalendarValueRepository.GetAll()
                                                     .Where(it => it.WatchDate > now && (it.ValidUntilDate == null || it.ValidUntilDate.Value.Date >= now.Date))
                                                     .ToList();            
            return watchDateCalendarValue;
        }

        public DeleteMessage DeleteWDCV(int id)
        {
            var wdcv = this._watchDateCalendarValueRepository.GetById(id);

            if (wdcv != null)
            {
                try
                {
                    this._watchDateCalendarValueRepository.Delete(wdcv);
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
        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
