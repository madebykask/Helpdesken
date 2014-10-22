namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Calendars
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Domain;

    public static class CalendarMapper
    {
        public static CalendarOverview[] MapToOverviews(this IQueryable<Calendar> query)
        {
            var entities = query.Select(c => new
                                        {
                                            c.Id,
                                            c.CalendarDate,
                                            c.Caption,
                                            c.Text
                                        }).ToArray();

            return entities.Select(c => new CalendarOverview
                                        {
                                            Id = c.Id,
                                            CalendarDate = c.CalendarDate,
                                            Caption = c.Caption,
                                            Text = c.Text
                                        }).ToArray();
        }
    }
}