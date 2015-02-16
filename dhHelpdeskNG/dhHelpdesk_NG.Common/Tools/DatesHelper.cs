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

        public static DateTime RoundToWorkDateTime(this DateTime date, int workingHour)
        {
            // quick and dirty fix for working hours 0 - 24
            workingHour = Math.Min(workingHour, 23);
            return new DateTime(date.Year, date.Month, date.Day, workingHour, 0, 0);
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

        public static bool HoursLessEqualDays(this int hours, int days)
        {
            return hours <= days * 24;
        }

        public static bool HoursGreaterDays(this int hours, int days)
        {
            return hours > days * 24;
        }
    }
}