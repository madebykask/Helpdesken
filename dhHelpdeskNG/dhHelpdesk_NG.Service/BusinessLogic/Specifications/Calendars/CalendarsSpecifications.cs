namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Calendars
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;

    public static class CalendarsSpecifications
    {
        public static IQueryable<Calendar> GetUntilToday(this IQueryable<Calendar> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(c => new DateTime(c.ShowUntilDate.Year, c.ShowUntilDate.Month, c.ShowUntilDate.Day) >= today);

            return query;
        } 
    }
}