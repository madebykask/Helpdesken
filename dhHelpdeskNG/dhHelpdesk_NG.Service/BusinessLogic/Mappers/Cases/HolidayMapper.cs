namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Domain;

    using LinqLib.Sequence;

    public static class HolidayMapper
    {
        public static HolidayOverview[] MapToOverviews(this IEnumerable<Holiday> query)
        {

            return query.Select(h => new HolidayOverview
                                            {
                                                HolidayDate = h.HolidayDate,
                                                TimeFrom = h.TimeFrom,
                                                TimeUntil = h.TimeUntil,
                                            }).ToArray();
        }

        public static HolidayOverview[] MapToOverviewsDept(this IEnumerable<Department> departments)
        {
            return
                departments.Where(it => it.HolidayHeader != null && it.HolidayHeader.Holidays.Any())
                    .SelectMany(
                        department => department.HolidayHeader.Holidays,
                        (department, holiday) =>
                        new HolidayOverview()
                            {
                                DepartmentId = department.Id,
                                HolidayDate = holiday.HolidayDate,
                                TimeFrom = holiday.TimeFrom,
                                TimeUntil = holiday.TimeUntil
                            })
                    .ToArray();
        }
    }
}