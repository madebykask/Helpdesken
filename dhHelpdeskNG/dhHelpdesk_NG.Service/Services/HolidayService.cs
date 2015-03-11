namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface IHolidayService
    {
        IEnumerable<Holiday> GetAll();

        IEnumerable<HolidayHeader> GetHolidayHeaders();

        IEnumerable<Holiday> GetHolidaysByHeaderId(int id);

        IEnumerable<Holiday> GetHolidaysByHeaderIdAndYear(int year, int id);

        IList<Holiday> GetHolidaysByHeaderIdAndYearForList(int year, int id);

        Holiday GetHoliday(int id);

        HolidayHeader GetHolidayHeader(int id);

        DeleteMessage DeleteHoliday(int id);

        void SaveHoliday(Holiday holiday, out IDictionary<string, string> errors);

        void SaveHolidayHeader(HolidayHeader holidayheader, out IDictionary<string, string> errors);

        IEnumerable<HolidayOverview> GetHolidayOverviews();

        IEnumerable<HolidayOverview> GetDefaultCalendar();
    }

    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IHolidayHeaderRepository _holidayHeaderRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IDepartmentRepository departmentRepository;

        public HolidayService(
            IHolidayRepository holidayRepository,
            IHolidayHeaderRepository holidayHeaderRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this._holidayRepository = holidayRepository;
            this._holidayHeaderRepository = holidayHeaderRepository;
            this._unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.departmentRepository = departmentRepository;
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

        public IEnumerable<Holiday> GetHolidaysByHeaderId(int id)
        {
            return this._holidayRepository.GetHolidaysByHeaderId(id).OrderBy(x => x.HolidayDate);
        }

        public IEnumerable<Holiday> GetHolidaysByHeaderIdAndYear(int year, int id)
        {
            return this._holidayRepository.GetHolidaysByHeaderIdAndYear(year, id).OrderBy(x => x.HolidayDate);
        }

        public IList<Holiday> GetHolidaysByHeaderIdAndYearForList(int year, int id)
        {
            //return this._holidayRepository.GetHolidaysByHeaderIdAndYearForList(year, id).OrderBy(x => x.HolidayDate);
            var query = (from h in this._holidayRepository.GetAll().Where(x => x.HolidayHeader_Id == id && x.HolidayDate.Year == year)
                         select h);


            return query.OrderBy(x => x.HolidayDate).ToList();
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

        public void SaveHolidayHeader(HolidayHeader holidayheader, out IDictionary<string, string> errors)
        {
            if (holidayheader == null)
                throw new ArgumentNullException("holidayheader");

            errors = new Dictionary<string, string>();

            if (holidayheader.Id == 0)
                this._holidayHeaderRepository.Add(holidayheader);
            else
                this._holidayHeaderRepository.Update(holidayheader);

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

        public IEnumerable<HolidayOverview> GetHolidayOverviews()
        {
            return this.departmentRepository.GetAll().MapToOverviewsDept();
        }

        public IEnumerable<HolidayOverview> GetDefaultCalendar()
        {
            const int DEFAULT_CALENDAR_ID = 1;
            return this._holidayRepository.GetHolidaysByHeaderId(DEFAULT_CALENDAR_ID).MapToOverviews();
        }

        private void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}