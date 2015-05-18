namespace DH.Helpdesk.Dal.Utils
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.WorkingDay;
    using DH.Helpdesk.Common.Tools;

    public class WorkTimeCalculator
    {
        private readonly TimeZoneInfo companyTimeZone;

        /// <summary>
        /// This structure contains work time info including work time in default calendar (id = 1)
        /// </summary>
        private Dictionary<DateTime, TimeRangesHolder> dailyWorkTime;

        /// <summary>
        /// Holds holiday work time information for department
        /// </summary>
        private DepartmentHolidayWorktimeMap departmentsWorkTime;

        public WorkTimeCalculator(TimeZoneInfo companyTimeZone)
        {
            this.companyTimeZone = companyTimeZone;
            this.departmentsWorkTime = new DepartmentHolidayWorktimeMap();
            this.dailyWorkTime = new Dictionary<DateTime, TimeRangesHolder>();
        }

        /// <summary>
        /// Sets data to use in calculations
        /// </summary>
        /// <param name="defaultWorkTime">default work time</param>
        /// <param name="departmentsHolidayData"></param>
        public void SetData(
            Dictionary<DateTime, TimeRangesHolder> defaultWorkTime,
            DepartmentHolidayWorktimeMap departmentsHolidayData)
        {
            this.dailyWorkTime = defaultWorkTime;
            this.departmentsWorkTime = departmentsHolidayData;
        }

        /// <summary>
        /// Calculates work time in minutes on specified time range
        /// </summary>
        /// <param name="fromUTC"></param>
        /// <param name="untilUTC"></param>
        /// <param name="caseDepartmentId"></param>
        /// <returns></returns>
        public int CalculateWorkTime(
            DateTime fromUTC,
            DateTime untilUTC,
            int? caseDepartmentId)
        {
            /// Achtung! when changing to daylight saving this two strings could throw exception
            var fetchFromLocal = TimeZoneInfo.ConvertTimeFromUtc(fromUTC, this.companyTimeZone);
            var fetchUntilLocal = TimeZoneInfo.ConvertTimeFromUtc(untilUTC, this.companyTimeZone);

            var calcFromDay = fetchFromLocal.RoundToDay();
            var calcToDay = fetchUntilLocal.RoundToDay();
            var fullDaysCount = (calcToDay - calcFromDay).Days;
            if (fullDaysCount == 0)
            {
                var dailyData = this.GetDailyData(calcFromDay, caseDepartmentId);
                //// calculations requested for the one day
                return dailyData == null
                           ? 0
                           : dailyData.Sum(fetchFromLocal, fetchUntilLocal);
            }

            var res = 0;
            var dateCounter = calcFromDay;
            do
            {
                var dailyData = this.GetDailyData(dateCounter, caseDepartmentId);
                if (dailyData != null)
                {
                    if (dateCounter == calcFromDay)
                    {
                        res += dailyData.Sum(fetchFromLocal, fetchUntilLocal.MakeTomorrow());
                    }
                    else if (dateCounter == calcToDay)
                    {
                        res += dailyData.Sum(fetchFromLocal.RoundToDay(), fetchUntilLocal);
                    }
                    else
                    {
                        res += dailyData.SummaryTime;
                    }
                }

                dateCounter = dateCounter.AddDays(1);
            }
            while (dateCounter <= calcToDay);

            return res;
        }

        public class RangesPerDay : Dictionary<DateTime, TimeRangesHolder>
        {
        }

        public class DepartmentHolidayWorktimeMap : Dictionary<int, RangesPerDay>
        {
        }

        private TimeRangesHolder GetDailyData(DateTime onDate, int? departmentId)
        {
            if (departmentId.HasValue && this.departmentsWorkTime.ContainsKey(departmentId.Value))
            {
                /// checking calendar for specific department
                if (this.departmentsWorkTime[departmentId.Value].ContainsKey(onDate))
                {
                    return this.departmentsWorkTime[departmentId.Value][onDate];
                }
            }
            else
            {
                /// checking "default" calendar
                if (this.dailyWorkTime.ContainsKey(onDate))
                {
                    return this.dailyWorkTime[onDate];
                }
            }

            return null;
        }
    }
}
