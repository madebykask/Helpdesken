using DH.Helpdesk.CaseSolutionScheduleYearly;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ScheduleCalculator
{
    public static DateTime? CalculateNextRun(CaseScheduleItem schedule, DateTime fromTime)
    {
        var now = fromTime;

        int hour = (int)schedule.ScheduleTime;
        int minutes = (int)((schedule.ScheduleTime - hour) * 60);
        var runTime = new TimeSpan(hour, minutes, 0);

        int yearStep = schedule.RepeatType == "EveryXYears" ? schedule.RepeatInterval ?? 1 : 1;
        int startYear = Math.Max(schedule.StartYear ?? now.Year, now.Year);

        var months = string.IsNullOrWhiteSpace(schedule.ScheduleMonths)
            ? Enumerable.Range(1, 12)
            : schedule.ScheduleMonths.Split(',').Select(int.Parse);

        var daysOfWeek = !string.IsNullOrWhiteSpace(schedule.DaysOfWeek)
            ? schedule.DaysOfWeek.Split(',').Select(int.Parse).ToHashSet()
            : null;

        var candidates = new List<DateTime>();

        for (int y = startYear; y <= startYear + 20; y += yearStep)
        {
            foreach (var m in months)
            {
                var days = new List<DateTime>();

                if ((schedule.ScheduleMonthlyOrder ?? 0) > 0 && (schedule.ScheduleMonthlyWeekday ?? 0) > 0)
                {
                    var special = GetNthWeekdayOfMonth(y, m, schedule.ScheduleMonthlyWeekday.Value, schedule.ScheduleMonthlyOrder.Value);
                    if (special.HasValue)
                        days.Add(special.Value);
                }
                else if ((schedule.ScheduleMonthlyDay ?? 0) > 0)
                {
                    int d = schedule.ScheduleMonthlyDay.Value;
                    if (DateTime.DaysInMonth(y, m) >= d)
                        days.Add(new DateTime(y, m, d));
                }
                else if (daysOfWeek != null)
                {
                    for (int d = 1; d <= DateTime.DaysInMonth(y, m); d++)
                    {
                        var date = new DateTime(y, m, d);
                        int dow = (int)date.DayOfWeek;
                        if (dow == 0) dow = 7;
                        if (daysOfWeek.Contains(dow))
                            days.Add(date);
                    }
                }

                foreach (var d in days)
                {
                    var candidate = d.Add(runTime);
                    if (candidate > now)
                        candidates.Add(candidate);
                }
            }
        }

        return candidates.OrderBy(x => x).FirstOrDefault();
    }

    private static DateTime? GetNthWeekdayOfMonth(int year, int month, int weekday, int order)
    {
        weekday %= 7;
        var first = new DateTime(year, month, 1);

        var firstMatch = Enumerable.Range(0, 7)
            .Select(i => first.AddDays(i))
            .FirstOrDefault(d => (int)d.DayOfWeek == weekday);

        if (order == 5)
        {
            var last = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return Enumerable.Range(0, 31)
                .Select(i => last.AddDays(-i))
                .FirstOrDefault(d => (int)d.DayOfWeek == weekday && d.Month == month);
        }

        return firstMatch.AddDays((order - 1) * 7);
    }
}
