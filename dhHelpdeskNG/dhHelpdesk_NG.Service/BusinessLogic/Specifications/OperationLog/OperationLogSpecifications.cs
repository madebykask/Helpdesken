namespace DH.Helpdesk.Services.BusinessLogic.Specifications.OperationLog
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;

    public static class OperationLogSpecifications
    {
        public static IQueryable<OperationLog> GetFromDate(this IQueryable<OperationLog> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(o => o.ShowDate <= today || o.ShowDate == null);

            return query;
        }

        public static IQueryable<OperationLog> GetUntilDate(this IQueryable<OperationLog> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(o => o.ShowUntilDate >= today || o.ShowUntilDate == null);

            return query;
        } 
    }
}