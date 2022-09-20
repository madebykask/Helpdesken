// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DateExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace DH.Helpdesk.Common.Extensions.DateTime
{
    using Constants;
    using System;
    using System.Threading;

    /// <summary>
    /// The date extensions.
    /// </summary>
    public static class DateExtensions
    {
        private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        /// <summary>
        /// The to formatted date.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToFormattedDate(this DateTime date)
        {
            return date.ToString("d", Thread.CurrentThread.CurrentUICulture);
        }

        public static string ToFormattedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                var d = date.Value;
                return d.ToString("d", Thread.CurrentThread.CurrentUICulture);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// The to formatted date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToFormattedDateTime(this DateTime date)
        {
            return date.ToString("g", Thread.CurrentThread.CurrentUICulture);
        }

        public static string ToMonthYear(this DateTime date)
        {
            return date.ToString("MM-yyyy");
        }

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }


        public static string ToSqlFormattedDate(this DateTime date)
        {
            return date.ToString("s", CultureInfo.InvariantCulture);
        }

        public static string ToSqlFormattedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("s", CultureInfo.InvariantCulture);
            }
            else
                return string.Empty;
        }



        public static DateTime RoundTick(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Kind);
        }

        public static bool IsValueChanged(this DateTime value)
        {
            return (value != NotChangedValue.DATETIME);
        }

        public static bool IsValueChanged(this DateTime? value)
        {
            return (value != NotChangedValue.NULLABLE_DATETIME);
        }

        public static DateTime? IfNullThenElse(this DateTime? value, DateTime? elseValue)
        {
            return value?? elseValue;
        }
    }
}