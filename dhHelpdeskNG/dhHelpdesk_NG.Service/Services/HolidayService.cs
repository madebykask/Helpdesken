namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IHolidayService
    {
        IEnumerable<Holiday> GetAll();
        IEnumerable<HolidayHeader> GetHolidayHeaders();

        IList<Holiday> GetHolidaysByHeaderId(int id);

        Holiday GetHoliday(int id);
        HolidayHeader GetHolidayHeader(int id);

        DeleteMessage DeleteHoliday(int id);

        void SaveHoliday(Holiday holiday, out IDictionary<string, string> errors);
        void Commit();

        /// <summary>
        /// The get holidays.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<HolidayOverview> GetHolidays();
    }

    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IHolidayHeaderRepository _holidayHeaderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HolidayService(
            IHolidayRepository holidayRepository,
            IHolidayHeaderRepository holidayHeaderRepository,
            IUnitOfWork unitOfWork)
        {
            this._holidayRepository = holidayRepository;
            this._holidayHeaderRepository = holidayHeaderRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Holiday> GetAll()
        {
            return this._holidayRepository.GetAll();
        }
                
        public IEnumerable<HolidayHeader> GetHolidayHeaders()
        {
            return this._holidayHeaderRepository.GetAll().OrderBy(x => x.Name);
        }

        public Holiday GetHoliday(int id)
        {
            return this._holidayRepository.GetById(id);
        }

        public IList<Holiday> GetHolidaysByHeaderId(int id)
        {
            return this._holidayRepository.GetHolidaysByHeaderId(id);
        }

        public HolidayHeader GetHolidayHeader(int id)
        {
            return this._holidayHeaderRepository.Get(x => x.Id == id);
        }

        public void SaveHoliday(Holiday holiday, out IDictionary<string, string> errors)
        {
            if (holiday == null)
                throw new ArgumentNullException("holiday");

            errors = new Dictionary<string, string>();

            if (holiday.Id == 0)
                this._holidayRepository.Add(holiday);
            else
                this._holidayRepository.Update(holiday);

            if (holiday.HolidayHeader.Id == 0)
                this._holidayHeaderRepository.Add(holiday.HolidayHeader);
            else
                this._holidayHeaderRepository.Update(holiday.HolidayHeader);

            if (errors.Count == 0)
                this.Commit();
        }

        public DeleteMessage DeleteHoliday(int id)
        {
            var holiday = this._holidayRepository.GetById(id);

            if (holiday != null)
            {
                try
                {
                    this._holidayRepository.Delete(holiday);
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

        /// <summary>
        /// The get holidays.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<HolidayOverview> GetHolidays()
        {
            return this._holidayRepository.GetHolidays();
        }
    }
}