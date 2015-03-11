namespace DH.Helpdesk.Mobile.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Utils;

    public static class WorkingTimeCalculatorFactory
    {
        /// <summary>
        /// Fabric method for create
        /// </summary>
        /// <param name="workingDayStart">in local time</param>
        /// <param name="workingDayEnd">in local time</param>
        /// <param name="holidays"></param>
        /// <param name="defaultHolidayCalendar"></param>
        /// <returns></returns>
        public static WorkTimeCalculator MakeCalculator(
            int workingDayStart,
            int workingDayEnd,
            IEnumerable<HolidayOverview> holidays,
            IEnumerable<HolidayOverview> defaultHolidayCalendar)
        {
            var holidayCache = new Dictionary<int, IList<HolidayOverview>>();
            foreach (var holidayOverview in holidays)
            {
                var deptId = holidayOverview.DepartmentId;
                if (holidayCache.ContainsKey(deptId))
                {
                    holidayCache[deptId].Add(holidayOverview);
                }
                else
                {
                    holidayCache.Add(deptId, new List<HolidayOverview>() { holidayOverview });
                }
            }

            //// due to we perform calculations in UTC, we need to conver working hours in UTC also
            //// Due to we have only hours there is a bug with NST (3:30) or New Zealand (2:45) timezones 
            //// TODO (Alexander Semenischev): fix bug with timezones
            var workBeginTime = DateTime.Now.RoundToWorkDateTime(workingDayStart).ToUniversalTime();
            var workEndTime = DateTime.Now.RoundToWorkDateTime(workingDayEnd).ToUniversalTime();
            var workBeginUtc = (int)Math.Floor((decimal)(workBeginTime.Hour * 60 + workBeginTime.Minute * 60 + workBeginTime.Second) / 60);
            var workEndUtc = (int)Math.Floor((decimal)(workEndTime.Hour * 60 + workEndTime.Minute * 60 + workEndTime.Second) / 60);
            return new WorkTimeCalculator(
                workBeginUtc,
                workEndUtc,
                holidayCache,
                defaultHolidayCalendar);
        }

        public static WorkTimeCalculator CreateFromWorkContext(IWorkContext workContext)
        {
            return MakeCalculator(
                 workContext.Customer.WorkingDayStart,
                 workContext.Customer.WorkingDayEnd,
                 workContext.Cache.Holidays,
                workContext.Cache.DefaultCalendarHolidays);
        }
    }
}