namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses
{
    using System.Linq;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;

    public static class ComputerSpecifications
    {
        public static IQueryable<Computer> GetByProduct(
                                            this IQueryable<Computer> query,
                                            IQueryable<Software> software, 
                                            IQueryable<Application> applications, 
                                            int customerId,
                                            int productId)
        {
            query =
                query.Where(c => c.Customer_Id == customerId)
                    .Where(
                        c =>
                        software.Where(
                            s =>
                            applications.Where(a => a.Products.Select(p => p.Id).Contains(productId))
                                .Select(a => a.Name)
                                .Contains(s.Name)).Select(s => s.Computer_Id).Contains(c.Id)); 

            return query;
        }
    }
}