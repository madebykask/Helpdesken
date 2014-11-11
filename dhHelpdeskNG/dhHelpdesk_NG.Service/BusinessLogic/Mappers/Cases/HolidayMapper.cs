namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Domain;

    public static class HolidayMapper
    {
        public static HolidayOverview[] MapToOverviews(this IQueryable<Holiday> query)
        {
            var entities = query.Select(h => new
                                            {
                                                h.HolidayDate,
                                                h.TimeFrom,
                                                h.TimeUntil,
                                                HolidayHeader = h.HolidayHeader.Name
                                            })
                                            .OrderBy(h => h.HolidayDate)
                                            .ToArray();

            return entities.Select(h => new HolidayOverview
                                            {
                                                HolidayDate = h.HolidayDate,
                                                TimeFrom = h.TimeFrom,
                                                TimeUntil = h.TimeUntil,
                                                HolidayHeader = new HolidayHeaderOverview
                                                {
                                                    Name = h.HolidayHeader
                                                }
                                            }).ToArray();
        }
    }
}