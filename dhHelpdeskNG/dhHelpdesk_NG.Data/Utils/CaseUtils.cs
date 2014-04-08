// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaseUtils.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CaseUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Utils
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;

    /// <summary>
    /// The case utils.
    /// </summary>
    public static class CaseUtils
    {
        /// <summary>
        /// The calculate lead time.
        /// </summary>
        /// <param name="caseRegistrationDate">
        /// The case registration date.
        /// </param>
        /// <param name="caseWatchDate">
        /// The case watch date.
        /// </param>
        /// <param name="caseExternalTime">
        /// The external time.
        /// </param>
        /// <param name="workingDayStart">
        /// The working day start.
        /// </param>
        /// <param name="workingDayEnd">
        /// The working day end.
        /// </param>
        /// <param name="holidays">
        /// The holidays.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static int CalculateLeadTime(
                                                DateTime caseRegistrationDate,
                                                DateTime? caseWatchDate,
                                                int caseExternalTime,
                                                int workingDayStart,
                                                int workingDayEnd,
                                                IEnumerable<HolidayOverview> holidays)
        {
            var startDate = caseRegistrationDate;
            var endDate = caseWatchDate.HasValue ? caseWatchDate.Value : DateTime.Now;

            if (startDate > endDate)
            {
                startDate = endDate;
            }

            var delay = new TimeSpan(0);

            return (int)(endDate - startDate - delay).TotalHours;
        }
    }
}