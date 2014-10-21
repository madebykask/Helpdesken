namespace DH.Helpdesk.Services.BusinessLogic.Specifications.OperationLogs
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OperationLogSpecifications
    {
        public static IQueryable<OperationLog> GetForStartPage(
                                    this IQueryable<OperationLog> query,
                                    int[] customers,
                                    int? count, 
                                    bool forStartPage)
        {
            if (customers != null && customers.Any())
            {
                query = query.Where(o => customers.Contains(o.Customer_Id));
            }

            if (forStartPage)
            {
                query = query.Where(o => o.ShowOnStartPage == 1);
            }

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }  
    }
}