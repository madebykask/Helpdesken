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
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Common.Tools;

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
            var startDate = caseRegistrationDate.RoundToHour();
            var endDate = (caseWatchDate.HasValue ? caseWatchDate.Value : DateTime.Now).RoundToHour();

            if (startDate > endDate)
            {
                startDate = endDate;
            }

            int holidaysHours = holidays
                .Where(holiday => startDate.RoundToDay() <= holiday.HolidayDate.RoundToDay() && 
                        holiday.HolidayDate.RoundToDay() <= endDate.RoundToDay())
                 .Sum(holiday => holiday.TimeUntil - holiday.TimeFrom);

            var allHours = DatesHelper.GetBusinessDays(startDate, endDate) * (workingDayEnd - workingDayStart);

            return allHours - holidaysHours - caseExternalTime;
        }
    }
}