namespace DH.Helpdesk.Services.BusinessLogic.Cases
{
    using System;

    using DH.Helpdesk.Domain;

    public static class CaseHelper
    {
        public static int? HoursLeftOnSolution(this Case c)
        {
            if (c == null)
            {
                return null;
            }

            if (!c.WatchDate.HasValue)
            {
                return null;
            }

            return (DateTime.Now - c.WatchDate).Value.Hours;
        }

        public static int? DaysLeftOnSolution(this Case c)
        {
            if (c == null)
            {
                return null;
            }

            if (!c.WatchDate.HasValue)
            {
                return null;
            }

            return (DateTime.Now - c.WatchDate).Value.Days;
        }
    }
}