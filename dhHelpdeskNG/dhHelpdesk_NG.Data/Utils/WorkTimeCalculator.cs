namespace DH.Helpdesk.Dal.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Common.Tools;

    /// <summary>
    /// Calculator for working time for cases
    /// </summary>
    public class WorkTimeCalculator
    {
        private readonly int workingHourBegin;

        private readonly int workingHourEnd;

        private readonly HolidayCache defaultCalendar;

        private readonly Dictionary<int, HolidayCache> depatmentsHoliday;

        /// <summary>
        /// Achtung!! workingHourBegin should be between 
        /// </summary>
        /// <param name="workingHourBegin"></param>
        /// <param name="workingHourEnd"></param>
        /// <param name="holidayCalendars"></param>
        /// <param name="defaultHolidayCalendar"></param>
        public WorkTimeCalculator(int workingHourBegin, int workingHourEnd, Dictionary<int, IList<HolidayOverview>> holidayCalendars, IEnumerable<HolidayOverview> defaultHolidayCalendar)
        {
            if (workingHourBegin < 0 || workingHourBegin > 23 || workingHourEnd < 0 || workingHourEnd > 24)
            {
                throw new ArgumentException();
            }

            this.workingHourBegin = workingHourBegin;
            this.workingHourEnd = workingHourEnd;
            this.depatmentsHoliday = new Dictionary<int, HolidayCache>();

            if (holidayCalendars != null)
            {
                foreach (var holidayKV in holidayCalendars)
                {
                    if (!this.depatmentsHoliday.ContainsKey(holidayKV.Key) && holidayKV.Value.Any())
                    {
                        var holidayCache = new HolidayCache();
                        foreach (var holidayOverview in holidayKV.Value)
                        {
                            /// temporary fix for exception in overview
                            if (!holidayCache.ContainsKey(holidayOverview.HolidayDate))
                            {
                                holidayCache.Add(
                                    holidayOverview.HolidayDate,
                                    new Tuple<int, int>(holidayOverview.TimeFrom, holidayOverview.TimeUntil));
                            }
                        }

                        this.depatmentsHoliday.Add(holidayKV.Key, holidayCache);
                    }
                }
            }

            if (defaultHolidayCalendar != null)
            {
                var holidayOverviews = defaultHolidayCalendar as HolidayOverview[] ?? defaultHolidayCalendar.ToArray();
                if (holidayOverviews.Any())
                {
                    this.defaultCalendar = new HolidayCache();
                    foreach (var el in holidayOverviews)
                    {
                        this.defaultCalendar.Add(el.HolidayDate, new Tuple<int, int>(el.TimeFrom, el.TimeUntil));
                    }
                }
            }
        }

        /// <summary>
        /// Fabric method for create
        /// </summary>
        /// <param name="workingDayStart">in local time</param>
        /// <param name="workingDayEnd">in local time</param>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public static WorkTimeCalculator MakeCalculator(
            int workingDayStart,
            int workingDayEnd,
            IEnumerable<HolidayOverview> holidays)
        {
            IEnumerable<HolidayOverview> defaultCalendar = null;
            var holidayCache = new Dictionary<int, IList<HolidayOverview>>();
            foreach (var holidayOverview in holidays)
            {
                var deptId = holidayOverview.HolidayHeader.DeptartmentId;
                if (holidayCache.ContainsKey(deptId))
                {
                    holidayCache[deptId].Add(holidayOverview);
                }
                else
                {
                    holidayCache.Add(deptId, new List<HolidayOverview>() { holidayOverview });
                }
            }

            //// default calendar with id = 1
            if (holidayCache.ContainsKey(1))
            {
                defaultCalendar = holidayCache[1];
            }

            //// due to we perform calculations in UTC, we need to conver working hours in UTC also
            //// Due to we have only hours there is a bug with NST (3:30) or New Zealand (2:45) timezones 
            //// TODO (Alexander Semenischev): fix bug with timezones
            var workBeginTime = DateTime.Now.RoundToWorkDateTime(workingDayStart).ToUniversalTime();
            var workEndTime = DateTime.Now.RoundToWorkDateTime(workingDayEnd).ToUniversalTime();
            var workBeginUtc = (int)Math.Floor((decimal)(workBeginTime.Hour * 60 + workBeginTime.Minute * 60 + workBeginTime.Second) / 60);
            var workEndUtc = (int)Math.Floor((decimal)(workEndTime.Hour * 60 + workEndTime.Minute * 60 + workEndTime.Second) / 60);
            return new WorkTimeCalculator(
                workBeginUtc,
                workEndUtc,
                holidayCache,
                defaultCalendar);
        }

        /// <summary>
        /// Returns time range intersection in minutes (inner join in terms of SQL)
        /// </summary>
        /// <param name="range1Begin"></param>
        /// <param name="range1End"></param>
        /// <param name="range2Begin"></param>
        /// <param name="range2End"></param>
        /// <returns></returns>
        public static int GetRange1Crossing(
            DateTime range1Begin,
            DateTime range1End,
            DateTime range2Begin,
            DateTime range2End)
        {
            if (range1Begin > range1End || range2Begin > range2End)
            {
                throw new ArgumentException();
            }

            if (range1End < range2Begin || range2End < range1Begin)
            {
                // range1 lies before or after range2
                return 0;
            }

            return (int)Math.Ceiling((((range1End > range2End) ? range2End : range1End) - (range1Begin > range2Begin ? range1Begin : range2Begin)).TotalMinutes);
        }

        /// <summary>
        /// Calculates working time on specified time period for department
        /// </summary>
        /// <param name="caseDepartmentId"></param>
        /// <param name="calcFrom">Period start in UTC</param>
        /// <param name="calcTo">Period until in UTC</param>
        /// <returns></returns>
        public int CalcWorkTimeMinutes(int? caseDepartmentId, DateTime calcFrom, DateTime calcTo)
        {
            if (calcFrom > calcTo)
            {
                throw new ArgumentException("calcFrom can not be more that calcTo");
            }

            HolidayCache holidaysCacheToUse = null;
            if (caseDepartmentId.HasValue && this.depatmentsHoliday.ContainsKey(caseDepartmentId.Value))
            {
                holidaysCacheToUse = this.depatmentsHoliday[caseDepartmentId.Value];
            }
            else
            {
                holidaysCacheToUse = this.defaultCalendar;
            }

            var commonWorkTime = CalcWorkTimeM(this.workingHourBegin, this.workingHourEnd, calcFrom, calcTo);
            var weekendTime = this.CalcWeekendTimeM(calcFrom, calcTo, holidaysCacheToUse);
            var holidayTime = (holidaysCacheToUse != null)
                                  ? this.CalcHolidayTimeM(calcFrom, calcTo, holidaysCacheToUse)
                                  : 0;

            return commonWorkTime - weekendTime - holidayTime;
        }



        private static int CalcWorkTimeM(int workingHourBegin, int workingHourEnd, DateTime calcFrom, DateTime calcTo)
        {
            var calcFromDay = calcFrom.RoundToDay();
            var calcToDay = calcTo.RoundToDay();
            var fullDaysCount = (calcToDay - calcFromDay).Days - 1;
            if (fullDaysCount == -1)
            {
                var calcFromRound = calcFrom.SetToHour(workingHourBegin);
                var calcToRound = calcFrom.SetToHour(workingHourEnd);
                //// here we have that calcFrom date == calcTo date
                return GetRange1Crossing(
                    calcFrom,
                    calcTo,
                    calcFromRound,
                    calcToRound);
            }

            var workingHoursPerDay = 24;
            var timePrefix = 0;
            var timePostfix = 0;
            if (workingHourBegin < workingHourEnd)
            {
                // working day like 8-18
                workingHoursPerDay = workingHourEnd - workingHourBegin;
                timePrefix = GetRange1Crossing(
                    calcFrom.SetToHour(workingHourBegin),
                    calcFrom.SetToHour(workingHourEnd),
                    calcFrom,
                    calcFrom.MakeTomorrow());
                timePostfix = GetRange1Crossing(
                    calcTo.SetToHour(workingHourBegin),
                    calcTo.SetToHour(workingHourEnd),
                    calcTo.RoundToDay(),
                    calcTo);
            }
            else if (workingHourBegin > workingHourEnd)
            {
                // working day like 20-3
                workingHoursPerDay = workingHourEnd + 24 - workingHourBegin;
                timePrefix = GetRange1Crossing(
                    calcFrom.SetToHour(workingHourBegin),
                    calcFrom.SetToHour(24),
                    calcFrom,
                    calcFrom.MakeTomorrow());
                timePostfix = GetRange1Crossing(
                    calcTo.RoundToDay(),
                    calcTo.SetToHour(workingHourEnd),
                    calcTo.RoundToDay(),
                    calcTo);
            }

            return workingHoursPerDay * fullDaysCount * 60 + timePrefix + timePostfix;
        }

        private static bool IsWeekendDay(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Calculates work time if it would usual working day.
        /// Works only for weeks with 2 days weekend
        /// </summary>
        /// <param name="calcFrom">Time in UTC</param>
        /// <param name="calcTo">Time in UTC</param>
        /// <param name="holidaysCache"></param>
        /// <returns></returns>
        private int CalcWeekendTimeM(DateTime calcFrom, DateTime calcTo, HolidayCache holidaysCache)
        {
            var dateCounter = calcFrom.RoundToDay();
            var dateToRounded = calcTo.RoundToDay();
            var wouldWorkTime = 0;
            const int WeekLen = 7;

            //// looking for the fist weekend day
            while (!(IsWeekendDay(dateCounter) || dateCounter == dateToRounded))
            {
                dateCounter = dateCounter.AddDays(1);
            }

            if (IsWeekendDay(dateCounter))
            {
                for (; dateCounter <= dateToRounded; dateCounter = dateCounter.AddDays((dateCounter.DayOfWeek == DayOfWeek.Sunday) ? 6 : WeekLen))
                {
                    var calcDateFrom = (calcFrom > dateCounter) ? calcFrom : dateCounter;
                    var calcDateTo =
                        calcDateFrom.AddDays((calcDateFrom.DayOfWeek == DayOfWeek.Sunday) ? 1 : 2).RoundToDay();

                    if (holidaysCache != null)
                    {
                        if (holidaysCache.ContainsKey(calcDateFrom.RoundToDay()))
                        {
                            //// here we have a case when first weekend is redday
                            if (calcDateFrom.DayOfWeek == DayOfWeek.Sunday)
                            {
                                continue;
                            }

                            if (holidaysCache.ContainsKey(calcDateFrom.AddDays(1).RoundToDay()))
                            {
                                //// and when second weekend is a redday too
                                continue;
                            }

                            calcFrom = calcDateFrom.AddDays(1);
                            calcTo = calcFrom.AddDays(1).RoundToDay();
                        }
                    }
                    
                    if (calcTo < calcDateTo)
                    {
                        calcDateTo = calcTo;
                    }

                    wouldWorkTime += CalcWorkTimeM(this.workingHourBegin, this.workingHourEnd, calcDateFrom, calcDateTo);
                }
            }

            return wouldWorkTime;
        }

        /// <summary>
        /// Calculates correction for working time in holiday
        /// </summary>
        /// <param name="calcFrom"></param>
        /// <param name="calcTo"></param>
        /// <param name="holidaysCache"></param>
        /// <returns></returns>
        private int CalcHolidayTimeM(DateTime calcFrom, DateTime calcTo, HolidayCache holidaysCache)
        {
            if (holidaysCache == null)
            {
                return 0;
            }

            var calcFromDay = calcFrom.RoundToDay();
            var calcToDay = calcTo.RoundToDay();
            
            // holds all time that we would spent to work if no holiday
            var workingTime = 0.0;

            // holds real working time that was spent in holidays
            var realWorkginTime = holidaysCache.Where(it => it.Key >= calcFromDay && it.Key <= calcToDay).Sum(
                it =>
                    {
                        var wHourBegin = it.Value.Item1;
                        var wHourEnd = it.Value.Item2;

                        if (it.Key == calcFromDay)
                        {
                            if (calcFromDay == calcToDay)
                            {
                                // At this point we have range in range case.
                                // this only possible if we have perform calculations in working hours in holiday 
                                workingTime =
                                    GetRange1Crossing(
                                        calcFrom,
                                        calcTo,
                                        calcFrom.SetToHour(this.workingHourBegin),
                                        calcTo.SetToHour(this.workingHourEnd));
                                return GetRange1Crossing(
                                    calcFrom,
                                    calcTo,
                                    calcFrom.SetToHour(wHourBegin),
                                    calcTo.SetToHour(wHourEnd));
                            }

                            workingTime =
                                GetRange1Crossing(
                                    calcFrom,
                                    calcFrom.MakeTomorrow(),
                                    calcFrom.SetToHour(this.workingHourBegin),
                                    calcFrom.SetToHour(this.workingHourEnd));
                            return GetRange1Crossing(
                                calcFrom,
                                calcFrom.MakeTomorrow(),
                                calcFrom.SetToHour(wHourBegin),
                                calcFrom.SetToHour(wHourEnd));
                        }
                        
                        if (it.Key == calcToDay)
                        {
                            workingTime =
                               GetRange1Crossing(
                                   calcToDay,
                                   calcTo,
                                   calcTo.SetToHour(this.workingHourBegin),
                                   calcTo.SetToHour(this.workingHourEnd));
                            return GetRange1Crossing(
                                calcTo,
                                calcTo.MakeTomorrow(),
                                calcTo.SetToHour(wHourBegin),
                                calcTo.SetToHour(wHourEnd));
                        }

                        workingTime += (this.workingHourEnd - this.workingHourBegin) * 60;
                        return (wHourEnd - wHourBegin) * 60;
                    });

            return (int)Math.Floor(workingTime - realWorkginTime);
        }
    

        private class HolidayCache : Dictionary<DateTime, Tuple<int, int>>
        {
        }
    }
}