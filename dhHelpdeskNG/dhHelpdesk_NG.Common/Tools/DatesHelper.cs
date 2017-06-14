// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatesHelper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DatesHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Common.Tools
{
    using System;

    /// <summary>
    /// The dates helper.
    /// </summary>
    public static class DatesHelper
    {
        public static DateTime RoundToMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// The round to day.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime RoundToDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime Min(DateTime compare1, DateTime compare2)
        {
            if (compare1 >= compare2)
            {
                return compare2;
            }

            return compare1;
        }

        public static DateTime Max(DateTime compare1, DateTime compare2)
        {
            if (compare1 >= compare2)
            {
                return compare1;
            }

            return compare2;
        }

        public static DateTime RoundToWorkDateTime(this DateTime date, int workingHour)
        {
            if (workingHour < 0 || workingHour > 23)
            {
                throw new ArgumentException("workingHour should be in 0..23 range");
            }

            return new DateTime(date.Year, date.Month, date.Day, workingHour, 0, 0);
        }

        /// <summary>
        /// Shortcut to round datetime to specified hour
        /// Achtung: if Hour == 24 sets date to next day
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static DateTime SetToHour(this DateTime date, int hour)
        {
            if (hour < 0 || hour > 24)
            {
                throw new ArgumentException();
            }

            if (hour == 24)
            {
                return new DateTime(date.Year, date.Month, date.Day).AddDays(1);
            }

            return new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
        }

        /// <summary>
        /// Difference in hours beteween usual time and daylight saving time
        /// </summary>
        private const int DAYLIGHTSAVING_HOUR_DIFF = 1;

        public static DateTime LocalToUtc(this DateTime date, TimeZoneInfo tz, bool takeLatestIfAmbiguous = false)
        {
            DateTime res;
            if (tz.IsAmbiguousTime(date))
            {
                res = takeLatestIfAmbiguous
                          ? DateTime.SpecifyKind(
                              date.AddMinutes(-(tz.BaseUtcOffset.TotalMinutes + DAYLIGHTSAVING_HOUR_DIFF * 60)),
                              DateTimeKind.Utc)
                          : DateTime.SpecifyKind(date - tz.BaseUtcOffset, DateTimeKind.Utc);
            }
            else
            {
                /// when begin or end of reange is inside daylight saving "lost" hour
                try
                {
                    res = TimeZoneInfo.ConvertTimeToUtc(date, tz);
                }
                catch (ArgumentException)
                {
                    res = TimeZoneInfo.ConvertTimeToUtc(date.AddHours(DAYLIGHTSAVING_HOUR_DIFF), tz);
                }
            }

            return res;
        }

        /// <summary>
        /// Makes next date for current date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MakeTomorrow(this DateTime date)
        {
            return date.RoundToDay().AddDays(1);
        }

        /// <summary>
        /// The round to hour.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime RoundToHour(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }

        public static int GetBusinessDays(DateTime startDate, DateTime endDate)
        {
            int businessDays = 0;
            var nextDay = startDate.RoundToDay();
            var end = endDate.RoundToDay();
            while (nextDay <= end)
            {
                if (nextDay.DayOfWeek != DayOfWeek.Saturday && 
                    nextDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;                    
                }

                nextDay = nextDay.AddDays(1);
            }

            return businessDays;
        }

        public static DateTime RoundToMonthOrGetCurrent(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return DateTime.Today.RoundToMonth();
            }

            return date.Value.RoundToMonth();
        }

        public static bool IsHoursLessDay(this int hours)
        {
            return hours < 24;
        }

        public static bool IsHoursLessEqualDays(this int hours, int days)
        {
            return hours <= days * 24;
        }

        public static bool IsHoursLessDays(this int hours, int days)
        {
            return hours < days * 24;
        }

        public static bool IsHoursGreaterEqualDays(this int hours, int days)
        {
            return hours >= days * 24;
        }

        public static bool IsHoursGreaterDays(this int hours, int days)
        {
            return hours > days * 24;
        }

        public static bool IsHoursEqualDays(this int hours, int days)
        {
            return hours.IsHoursGreaterEqualDays(days) && hours.IsHoursLessDays(days + 1);
        }

        public static bool IsHoursLessDay(this int hours, int workingHours)
        {
            return hours < workingHours;
        }

        public static bool IsHoursLessEqualDays(this int hours, int days, int workingHours)
        {
            return hours <= days * workingHours;
        }

        public static bool IsHoursLessDays(this int hours, int days, int workingHours)
        {
            return hours < days * workingHours;
        }

        public static bool IsHoursGreaterEqualDays(this int hours, int days, int workingHours)
        {
            return hours >= days * workingHours;
        }

        public static bool IsHoursGreaterDays(this int hours, int days, int workingHours)
        {
            return hours > days * workingHours;
        }

        public static bool IsHoursEqualDays(this int hours, int days, int workingHours)
        {
            return hours.IsHoursGreaterEqualDays(days, workingHours) && hours.IsHoursLessDays(days + 1, workingHours);
        }

        /// <summary>
        /// Returns end of day.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetEndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime? GetEndOfDay(this DateTime? date)
        {
            return date.HasValue ? new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59) : (DateTime?) null;
        }
    }
}