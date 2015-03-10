// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HolidayOverview.cs" company="">
//   
// </copyright>
// <summary>
//   The holiday overview.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace DH.Helpdesk.BusinessData.Models.Holiday.Output
{
    using System;

    /// <summary>
    /// The holiday overview.
    /// </summary>
    public sealed class HolidayOverview
    {
        /// <summary>
        /// Gets or sets the time from.
        /// </summary>
        public int TimeFrom { get; set; }

        /// <summary>
        /// Begin of working day in holiday
        /// </summary>
        public int TimeUntil { get; set; }

        /// <summary>
        /// End of working day in holiday
        /// </summary>
        public DateTime HolidayDate { get; set; }

        /// <summary>
        /// Gets or sets the holiday header.
        /// </summary>
        public HolidayHeaderOverview HolidayHeader { get; set; }
    }
}