namespace DH.Helpdesk.Services.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.WorkingDay;
    using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Services.Services;

    /// <summary>
    ///
    /// </summary>
    public class WorkTimeCalculatorFactory : IWorkTimeCalculatorFactory
    {
        private readonly IHolidayService _holidayService;

        private readonly int _workHourBegin;

        private readonly int _workHourEnd;

        private readonly TimeZoneInfo _companyTimeZone;

        public WorkTimeCalculatorFactory(
            IHolidayService holidayService,
            int workHourBegin,
            int workHourEnd,
            TimeZoneInfo companyTimeZone)
        {
            _holidayService = holidayService;
            _workHourBegin = workHourBegin;
            _workHourEnd = workHourEnd;
            _companyTimeZone = companyTimeZone;
        }

        public WorkTimeCalculator.RangesPerDay CreateWorkDaysSchema(
            DateTime from,
            DateTime to,
            int workHourBegin,
            int workHourEnd,
            Dictionary<DateTime, Tuple<int, int>> holidays)
        {
            var dtCounterLocal = from.RoundToDay();
            var fetchUntil = to.MakeTomorrow();
            var stdWorkSchema = new WorkTimeCalculator.RangesPerDay();
            var isTwoRanges = workHourBegin > workHourEnd
                        || (workHourBegin == workHourEnd && workHourBegin != 0);
            do
            {
                if (holidays.ContainsKey(dtCounterLocal))
                {
                    var holiday = holidays[dtCounterLocal];
                    if (holiday.Item1 != 0 && holiday.Item2 != 0)
                    {
                        if ((holiday.Item1 == holiday.Item2 && holiday.Item1 != 0) || (holiday.Item1 > holiday.Item2))
                        {
                            var nextday = dtCounterLocal.MakeTomorrow();
                            this.AddOrCreateIfNotExists(
                                stdWorkSchema,
                                dtCounterLocal,
                                new TimeRange() { begin = dtCounterLocal.SetToHour(holiday.Item1), end = nextday });
                            this.AddOrCreateIfNotExists(
                                stdWorkSchema,
                                nextday,
                                new TimeRange() { begin = nextday, end = nextday.SetToHour(holiday.Item2) });
                        }
                        else
                        {
                            this.AddOrCreateIfNotExists(
                                stdWorkSchema,
                                dtCounterLocal,
                                new TimeRange()
                                {
                                    begin = dtCounterLocal.SetToHour(holiday.Item1),
                                    end = dtCounterLocal.SetToHour(holiday.Item2)
                                });
                        }
                    }
                }
                else
                {
                    if (!IsWeekend(dtCounterLocal))
                    {
                        if (isTwoRanges)
                        {
                            var nextday = dtCounterLocal.MakeTomorrow();
                            this.AddOrCreateIfNotExists(
                                stdWorkSchema,
                                dtCounterLocal,
                                new TimeRange() { begin = dtCounterLocal.SetToHour(workHourBegin), end = nextday });
                            this.AddOrCreateIfNotExists(
                                stdWorkSchema,
                                nextday,
                                new TimeRange() { begin = nextday, end = nextday.SetToHour(workHourEnd) });
                        }
                        else
                        {
                            this.AddOrCreateIfNotExists(
                                stdWorkSchema,
                                dtCounterLocal,
                                new TimeRange()
                                {
                                    begin = dtCounterLocal.SetToHour(workHourBegin),
                                    end = dtCounterLocal.SetToHour(workHourEnd)
                                });
                        }
                    }
                }

                dtCounterLocal = dtCounterLocal.AddDays(1);
            }
            while (dtCounterLocal < fetchUntil);
            return stdWorkSchema;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rangeBeginUtc"></param>
        /// <param name="rangeEndUtc"></param>
        /// <param name="departmentsIds">list of departments to fetch calendar data for</param>
        /// <param name="timeDiff"></param>
        /// <returns></returns>
        public WorkTimeCalculator Build(
            DateTime rangeBeginUtc,
            DateTime rangeEndUtc,
            int[] departmentsIds,
            int timeDiff = 0)
        {
            var res = new WorkTimeCalculator(this._companyTimeZone);
            // @TODO: Can throw exception when UTC inside daylight saving hour. Fix it
            var rangeBegin = TimeZoneInfo.ConvertTimeFromUtc(rangeBeginUtc, this._companyTimeZone);
            var rangeEnd = TimeZoneInfo.ConvertTimeFromUtc(rangeEndUtc, this._companyTimeZone);

            if (timeDiff != 0)
            {
                rangeBegin = rangeBeginUtc.AddMinutes(timeDiff);
                rangeEnd = rangeEndUtc.AddMinutes(timeDiff);
            }

            // holidays
            var departmentsHolidays = _holidayService.GetHolidayBetweenDatesForDepartments(
                rangeBegin.RoundToDay(),
                rangeEnd,
                departmentsIds);

            var departmentsHolidaysData = new Dictionary<int, Dictionary<DateTime, Tuple<int, int>>>();
            if (departmentsHolidays != null)
            {
                //  Dictionary<int, Dictionary<DateTime, TimeRangesHolder>>
                foreach (var deptHoliday in departmentsHolidays)
                {
                    if (departmentsHolidaysData.ContainsKey(deptHoliday.DepartmentId))
                    {
                        if (!departmentsHolidaysData[deptHoliday.DepartmentId].ContainsKey(deptHoliday.HolidayDate))
                        {
                            departmentsHolidaysData[deptHoliday.DepartmentId].Add(
                            deptHoliday.HolidayDate,
                            new Tuple<int, int>(deptHoliday.TimeFrom, deptHoliday.TimeUntil));
                        }
                    }
                    else
                    {
                        departmentsHolidaysData[deptHoliday.DepartmentId] = new Dictionary<DateTime, Tuple<int, int>>
                                                                                {
                                                                                    {
                                                                                        deptHoliday.HolidayDate,
                                                                                        new Tuple<int, int>(deptHoliday.TimeFrom, deptHoliday.TimeUntil)
                                                                                    }
                                                                                };
                    }
                }
            }

            var defaultCal = _holidayService.GetDefaultCalendar()
                    .ToDictionary(it => it.HolidayDate, it => new Tuple<int, int>(it.TimeFrom, it.TimeUntil));
            var stdWorkSchema = CreateWorkDaysSchema(
                                rangeBegin,
                                rangeEnd,
                                _workHourBegin,
                                _workHourEnd,
                                defaultCal);
            var deptWorkSchema = new WorkTimeCalculator.DepartmentHolidayWorktimeMap();
            foreach (var deptHoliday in departmentsHolidaysData)
            {
                deptWorkSchema.Add(deptHoliday.Key, CreateWorkDaysSchema(rangeBegin, rangeEnd, _workHourBegin, _workHourEnd, deptHoliday.Value));
            }

            res.SetData(stdWorkSchema, deptWorkSchema);
            return res;
        }

        private static bool IsWeekend(DateTime onDate)
        {
            return onDate.DayOfWeek == DayOfWeek.Saturday || onDate.DayOfWeek == DayOfWeek.Sunday;
        }

        private void AddOrCreateIfNotExists(Dictionary<DateTime, TimeRangesHolder> dt, DateTime onDate, TimeRange item)
        {
            if (dt.ContainsKey(onDate))
            {
                dt[onDate].Add(item);
            }
            else
            {
                dt[onDate] = new TimeRangesHolder() { WorkingHours = new[] { item } };
            }
        }
    }
}