namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderEditMapper
    {
        public static OrderEditOptions MapToOrderEditOptions(
                                        IQueryable<OrderState> statuses,
                                        IQueryable<User> administrators,
                                        IQueryable<Domain> domains,
                                        IQueryable<Department> departments,
                                        IQueryable<OU> units,
                                        IQueryable<OrderPropertyEntity> properties,
                                        IQueryable<Department> deliveryDepartments,
                                        IQueryable<OU> deliveryOuIds,
                                        FullOrderEditSettings settings)
        {
            return null;
        }
    }
}