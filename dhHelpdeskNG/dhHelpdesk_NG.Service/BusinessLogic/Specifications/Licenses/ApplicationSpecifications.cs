namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class ApplicationSpecifications
    {
        public static IQueryable<Application> GetOnlyConnectedApplications(
                                        this IQueryable<Application> query,
                                        bool onlyConnected)
        {
            if (onlyConnected)
            {
                query = query.Where(a => a.Products.Any());
            }

            return query;
        }

        public static IQueryable<Application> GetOnlyConnectedCustomerApplications(
                                        this IQueryable<Application> query,
                                        int customerId,
                                        bool onlyConnected)
        {
            query = query
                .GetByCustomer(customerId)
                .GetOnlyConnectedApplications(onlyConnected);

            return query;
        } 
    }
}