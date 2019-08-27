using Ninject.Infrastructure.Language;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

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
        
        IEnumerable<HolidayOverview> GetDefaultCalendar();

        /// <summary>
        /// Returns holidays between specified date and for specified department.
        /// Time range should be in customers timezone.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="departmentsIds"></param>
        /// <returns></returns>
        IEnumerable<HolidayOverview> GetHolidayBetweenDatesForDepartments(
            DateTime from,
            DateTime to,
           int[] departmentsIds);
    }

    public class HolidayService : IHolidayService
    {
        /// <summary>
        /// ID of default calendar for all customers
        /// </summary>
        public const int DEFAULT_CALENDAR_ID = 1;

        private readonly IHolidayRepository _holidayRepository;

        private readonly IHolidayHeaderRepository _holidayHeaderRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IDepartmentService _departmentService;

        public HolidayService(
            IHolidayRepository holidayRepository,
            IHolidayHeaderRepository holidayHeaderRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentService departmentService)
        {
            this._holidayRepository = holidayRepository;
            this._holidayHeaderRepository = holidayHeaderRepository;
            this._unitOfWork = unitOfWork;
            this._departmentService = departmentService;
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

        /// <summary>
        /// @TODO: code review - implement fetching logic inside this method, not in repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            var query = (from h in this._holidayRepository.GetMany(x => x.HolidayHeader_Id == id && x.HolidayDate.Year == year)
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
        

        public IEnumerable<HolidayOverview> GetDefaultCalendar()
        {
            return this._holidayRepository.GetHolidaysByHeaderId(DEFAULT_CALENDAR_ID).MapToOverviews();
        }

        public IEnumerable<HolidayOverview> GetHolidayBetweenDatesForDepartments(DateTime @from, DateTime to, int[] departmentsIds)
        {
            if (departmentsIds == null)
            {
                return null;
            }

            if (@from > to)
            {
                throw new ArgumentException("@from date can not be more that to");
            }

            //TODO: query can be optimized by filtering from,to. But departments which have calendar but not in range(from,to) also must be included
            var allDepsWithCalendars = this._departmentService.GetDepartmentsByIdsWithHolidays(departmentsIds, DEFAULT_CALENDAR_ID);
            var depsWithCalendarsWithNoHolidays = allDepsWithCalendars.Where(d => d.HolidayHeader.Holidays.Count == 0).ToArray();

            var res = allDepsWithCalendars.SelectMany(
                    department => department.HolidayHeader.Holidays,
                    (department, holiday) =>
                        new HolidayOverview()
                        {
                            DepartmentId = department.Id,
                            HolidayDate = holiday.HolidayDate,
                            TimeFrom = holiday.TimeFrom,
                            TimeUntil = holiday.TimeUntil
                        }).ToList();
            //adding fake  HolidayOverview for calendars without holidays, because in SelectMany they are not included
            //TODO: refactor this implementation
            if (depsWithCalendarsWithNoHolidays.Any())
            {
                res.AddRange(depsWithCalendarsWithNoHolidays.Select(d => new HolidayOverview()
                {
                    DepartmentId = d.Id,
                    HolidayDate = new DateTime(1900,1,1),
                    TimeFrom = 0,
                    TimeUntil = 0
                }));
            }

            return res;
        }

        private void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}