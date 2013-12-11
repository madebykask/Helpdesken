using System;
using System.Collections.Generic;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using System.Linq;

namespace dhHelpdesk_NG.Service
{
    public interface IHolidayService
    {
        IEnumerable<Holiday> GetAll();
        IEnumerable<HolidayHeader> GetHolidayHeaders();

        Holiday GetHoliday(int id);
        HolidayHeader GetHolidayHeader(int id);

        void SaveHoliday(Holiday holiday, out IDictionary<string, string> errors);
        void Commit();
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
            _holidayRepository = holidayRepository;
            _holidayHeaderRepository = holidayHeaderRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Holiday> GetAll()
        {
            return _holidayRepository.GetAll();
        }
                
        public IEnumerable<HolidayHeader> GetHolidayHeaders()
        {
            return _holidayHeaderRepository.GetAll().OrderBy(x => x.Name);
        }

        public Holiday GetHoliday(int id)
        {
            return _holidayRepository.GetById(id);
        }

        public HolidayHeader GetHolidayHeader(int id)
        {
            return _holidayHeaderRepository.Get(x => x.Id == id);
        }

        public void SaveHoliday(Holiday holiday, out IDictionary<string, string> errors)
        {
            if (holiday == null)
                throw new ArgumentNullException("holiday");

            errors = new Dictionary<string, string>();

            if (holiday.Id == 0)
                _holidayRepository.Add(holiday);
            else
                _holidayRepository.Update(holiday);

            if (holiday.HolidayHeader.Id == 0)
                _holidayHeaderRepository.Add(holiday.HolidayHeader);
            else
                _holidayHeaderRepository.Update(holiday.HolidayHeader);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}