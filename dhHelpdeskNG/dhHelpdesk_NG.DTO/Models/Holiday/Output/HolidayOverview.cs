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
        /// Begin of work in holiday in users local time
        /// </summary>
        public int TimeFrom { get; set; }

        /// <summary>
        /// End of work in holiday in users local time
        /// </summary>
        public int TimeUntil { get; set; }

        /// <summary>
        /// Date of holiday in users local time
        /// </summary>
        public DateTime HolidayDate { get; set; }

        /// <summary>
        /// Gets or sets the holiday header.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Resolves whether this holiday is a "working" holiday.
        /// 0 - 24 means 24h working holiday
        /// </summary>
        /// <returns></returns>
        public bool IsWorkingHoliday()
        {
            return !(this.TimeFrom == 0 && this.TimeUntil == 0);
        }
    }
}