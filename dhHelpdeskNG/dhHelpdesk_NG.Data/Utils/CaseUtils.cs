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
        /// The time left in hours
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

        /// <summary>
        /// Calcualtes time in hours left before case should be closed
        /// </summary>
        /// <param name="caseRegistrationDate"></param>
        /// <param name="caseFinishingDate"></param>
        /// <param name="caseShouldBeFinishedInDate">Date when case should be finished</param>
        /// <param name="workingDayStart"></param>
        /// <param name="workingDayEnd"></param>
        /// <param name="holidays"></param>
        /// <param name="SLAtime">time in miuntues</param>
        /// <param name="timeOnPause">time in minutes when case was "on Hold"</param>
        /// <returns>Time in hours. Can be null if no caseFinishingDate is null</returns>
        public static int? CalculateTimeLeft(
            DateTime caseRegistrationDate, 
            DateTime? caseFinishingDate,
            DateTime? caseShouldBeFinishedInDate,
            int workingDayStart,
            int workingDayEnd,
            IEnumerable<HolidayOverview> holidays,
            int SLAtime = 0,
            int timeOnPause = 0)
        {
            if (caseFinishingDate.HasValue)
            {
                return null;
            }
           
            if (caseShouldBeFinishedInDate.HasValue)
            {
                /// caseShouldBeFinishedInDate has time like 00:00:00,
                /// but we need to know exactly the latest time of that day
                var caseShoudlBeFinishedWithTime = caseShouldBeFinishedInDate.Value.AddHours(workingDayEnd);
                var now = DateTime.UtcNow;
                int workingTimeLeft;
                if (caseShoudlBeFinishedWithTime > now)
                {
                    workingTimeLeft = CalculateTotalWorkingMinutes(
                        now,
                        caseShoudlBeFinishedWithTime,
                        workingDayStart,
                        workingDayEnd,
                        holidays);
                }
                else
                {
                    workingTimeLeft = -CalculateTotalWorkingMinutes(
                       caseShoudlBeFinishedWithTime,
                       now,
                       workingDayStart,
                       workingDayEnd,
                       holidays);
                }
                
                return workingTimeLeft / 60;
            }

            if (SLAtime == 0)
            {
                return null;
            }
            
            var timeSpent = CalculateTotalWorkingMinutes(
                                caseRegistrationDate,
                                DateTime.UtcNow,
                                workingDayStart,
                                workingDayEnd,
                                holidays);
            SLAtime = SLAtime - timeSpent + timeOnPause;
            return SLAtime / 60;
        }

        /// <summary>
        /// Calculates working time in minutes for specified interval. Takes in account working hours and holidays and weekends (only Saturday and Sunday)
        /// </summary>
        /// <param name="startDate">time in UTC</param>
        /// <param name="endDate">time in UTC</param>
        /// <param name="workingHourBegin">hour when working day starts at (ie 8)</param>
        /// <param name="workingHourEnd">hour when working day ends (ie 17)</param>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public static int CalculateTotalWorkingMinutes(
                                DateTime startDate,
                                DateTime endDate,
                                int workingHourBegin,
                                int workingHourEnd,
                                IEnumerable<HolidayOverview> holidays)
        {
            if (workingHourEnd <= workingHourBegin)
            {
                throw new Exception("Bad parameters workingHourBegin or workingHourEnd");
            }

            if (startDate >= endDate)
            {
                return 0;
            }

            int holidaysMinutes = holidays != null
                                      ? holidays.Where(
                                          holiday =>
                                          startDate.RoundToDay() <= holiday.HolidayDate.RoundToDay()
                                          && holiday.HolidayDate.RoundToDay() <= endDate.RoundToDay()
                                          && holiday.HolidayDate.DayOfWeek != DayOfWeek.Saturday
                                          && holiday.HolidayDate.DayOfWeek != DayOfWeek.Sunday)
                                            .Sum(holiday => (holiday.TimeUntil - holiday.TimeFrom) * 60)
                                      : 0;

            int prefixTimeMins = 0;
            DateTime tunedStartDate;
            DateTime tunedEndDate;
            
            if (startDate.Hour <= workingHourBegin)
            {
                startDate = startDate.RoundToWorkDateTime(workingHourBegin);
                tunedStartDate = startDate.RoundToWorkDateTime(workingHourBegin);
            }
            else
            {
                prefixTimeMins += ((workingHourEnd - Math.Min(startDate.Hour, workingHourEnd)) * 60)
                                    + startDate.Minute;
                tunedStartDate = startDate.RoundToWorkDateTime(workingHourBegin).AddDays(1);
            }

            if (endDate.Hour >= workingHourEnd)
            {
                endDate = endDate.RoundToWorkDateTime(workingHourEnd);
                tunedEndDate = endDate.RoundToWorkDateTime(workingHourEnd);
            }
            else
            {
                prefixTimeMins += ((Math.Max(endDate.Hour, workingHourBegin) - workingHourBegin) * 60)
                                    + endDate.Minute;
                tunedEndDate = endDate.RoundToWorkDateTime(workingHourEnd).AddDays(-1);
            }

            if (startDate.RoundToDay() == endDate.RoundToDay())
            {                
                return Convert.ToInt32(Math.Ceiling((endDate - startDate).TotalMinutes));
            }
            
            var businessDays = DatesHelper.GetBusinessDays(tunedStartDate.RoundToDay(), tunedEndDate.RoundToDay());
            var workingMinutes = prefixTimeMins + (businessDays * (workingHourEnd - workingHourBegin) * 60);
            return Math.Max(workingMinutes - holidaysMinutes, 0);
        }
    }
}