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
        /// <param name="caseFinishingDate">
        /// The case finishing date.
        /// </param>
        /// <param name="caseExternalTime">
        /// The case external time.
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
        /// The <see cref="int"/>.
        /// </returns>
        public static int CalculateLeadTime(
                                                DateTime caseRegistrationDate,
                                                DateTime? caseWatchDate,
                                                DateTime? caseFinishingDate,
                                                int caseExternalTime,
                                                int workingDayStart,
                                                int workingDayEnd,
                                                IEnumerable<HolidayOverview> holidays)
        {
            var startDate = caseRegistrationDate.RoundToHour();
            DateTime endDate;
            if (caseFinishingDate.HasValue)
            {
                endDate = caseFinishingDate.Value.RoundToHour();
            }
            else
            {
                endDate = (caseWatchDate.HasValue ? caseWatchDate.Value : DateTime.Now).RoundToHour();                
            }                

            if (startDate > endDate)
            {
                startDate = endDate;
            }

            var workingMinutes = CalculateTotalWorkingMinutes(
                                    startDate,
                                    endDate,
                                    workingDayStart,
                                    workingDayEnd,
                                    holidays);

            return (workingMinutes - caseExternalTime) / 60;
        }

        public static int CalculateTotalWorkingMinutes(
                                DateTime startDate,
                                DateTime endDate,
                                int workingDayStart,
                                int workingDayEnd,
                                IEnumerable<HolidayOverview> holidays)
        {
            int holidaysMinutes = holidays != null ? holidays
                .Where(holiday => startDate.RoundToDay() <= holiday.HolidayDate.RoundToDay() &&
                        holiday.HolidayDate.RoundToDay() <= endDate.RoundToDay() &&
                        holiday.HolidayDate.DayOfWeek != DayOfWeek.Saturday &&
                        holiday.HolidayDate.DayOfWeek != DayOfWeek.Sunday)
                        .Sum(holiday => (holiday.TimeUntil - holiday.TimeFrom) * 60) : 0;

            var businessDays = DatesHelper.GetBusinessDays(startDate, endDate);

            var workingMinutes = businessDays > 1 ?
                businessDays * (workingDayEnd - workingDayStart) * 60 : 
                (int)(endDate - startDate).TotalMinutes;                

            return workingMinutes > holidaysMinutes ? 
                workingMinutes - holidaysMinutes : 0;
        }
    }
}