namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Calendars
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;

    public static class CalendarsSpecifications
    {
        public static IQueryable<Calendar> GetFromDate(this IQueryable<Calendar> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(c => c.ShowFromDate <= today);

            return query;
        } 

        public static IQueryable<Calendar> GetUntilDate(this IQueryable<Calendar> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(c => c.ShowUntilDate >= today);

            return query;
        } 
    }
}