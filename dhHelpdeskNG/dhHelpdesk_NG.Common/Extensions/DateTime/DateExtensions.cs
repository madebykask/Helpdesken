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
    using System.Threading;

    /// <summary>
    /// The date extensions.
    /// </summary>
    public static class DateExtensions
    {
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
    }
}