namespace DH.Helpdesk.Services.BusinessLogic.Specifications.OperationLogs
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OperationLogSpecifications
    {
        public static IQueryable<OperationLog> GetSorted(this IQueryable<OperationLog> query)
        {
            query = query.OrderByDescending(o => o.CreatedDate); 

            return query;
        }  
    }
}