namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Changes
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;

    public static class ChangeMapper
    {
        public static CustomerChanges[] MapToCustomerChanges(this IQueryable<Customer> query, int userId)
        {
            var entities = query.Select(cus => new
                                            {
                                                CustomerId = cus.Id,
                                                CustomerName = cus.Name,
                                                ChangesInProgress = cus.Changes.Where(c => c.ChangeStatus == null || c.ChangeStatus.CompletionStatus == 0).Count(),
                                                ChangesClosed = cus.Changes.Where(c => c.ChangeStatus != null && c.ChangeStatus.CompletionStatus != 0).Count(),
                                                ChangesForUser = cus.Changes.Where(c => c.User_Id == userId).Count()
                                            }).ToArray();

            var overviews = entities.Select(c => new CustomerChanges(
                                                c.CustomerId,
                                                c.CustomerName,
                                                c.ChangesInProgress,
                                                c.ChangesClosed,
                                                c.ChangesForUser)).ToArray();

            return overviews;
        }
    }
}