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

        /// <summary>
        /// The get business days.
        /// </summary>
        /// <param name="startD">
        /// The start d.
        /// </param>
        /// <param name="endD">
        /// The end d.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
            1 + ((endD - startD).TotalDays * 5 - (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if ((int)endD.DayOfWeek == 6)
            {
                calcBusinessDays--;
            }

            if ((int)startD.DayOfWeek == 0)
            {
                calcBusinessDays--;
            }

            return (int)calcBusinessDays;
        }
    }
}