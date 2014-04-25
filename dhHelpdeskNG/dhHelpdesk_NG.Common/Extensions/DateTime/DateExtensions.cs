// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DateExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Common.Extensions.DateTime
{
    using System;

    /// <summary>
    /// The date extensions.
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// The format date.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatDate(this DateTime d)
        {
            var ret = string.Empty;

            if (d != DateTime.MinValue)
            {
                ret = d.ToShortDateString();
            }

            return ret;
        }

        /// <summary>
        /// The get date format.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDateFormat()
        {
            return "yyyy-MM-dd";
        }

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
            return date.ToString("dd-MM-yyyy");
        }

        /// <summary>
        /// The to formatted date reverse.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToFormattedDateReverse(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
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
            return date.ToString("dd-MM-yyyy H:mm");
        }

        /// <summary>
        /// The to formatted date time reverse.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToFormattedDateTimeReverse(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd H:mm");
        }
    }
}