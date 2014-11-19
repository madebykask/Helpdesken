namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Orders
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class OrderFieldSettingsSpecifications
    {
        public static IQueryable<OrderFieldSettings> GetByType(
                                this IQueryable<OrderFieldSettings> query,
                                int customerId,
                                int? orderTypeId)
        {
            if (!orderTypeId.HasValue)
            {
                query = query
                    .GetByCustomer(customerId)
                    .Where(f => !f.OrderType_Id.HasValue);

                return query;
            }

            query = query
                    .GetByCustomer(customerId)
                    .Where(f => f.OrderType_Id == orderTypeId);

            return query;
        }
    }
}