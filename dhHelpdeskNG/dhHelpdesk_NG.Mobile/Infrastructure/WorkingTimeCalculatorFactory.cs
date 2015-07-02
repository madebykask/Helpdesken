namespace DH.Helpdesk.Mobile.Infrastructure
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
    using DH.Helpdesk.Dal.Infrastructure.Context;

    public static class WorkingTimeCalculatorFactory
    {
        /// <summary>
        /// Fabric method for create
        /// </summary>
        /// <param name="workingDayStart">In customers time zone</param>
        /// <param name="workingDayEnd">In customers time zone</param>
        /// <param name="holidays">In customers time zone</param>
        /// <param name="defaultHolidayCalendar">In customers time zone</param>
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

            return null;
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