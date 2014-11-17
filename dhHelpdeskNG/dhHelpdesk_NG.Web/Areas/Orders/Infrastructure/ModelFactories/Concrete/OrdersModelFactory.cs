namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class OrdersModelFactory : IOrdersModelFactory
    {
        public OrdersIndexModel GetIndexModel(OrdersFilterData data, OrdersFilterModel filter)
        {
            var orderTypes = WebMvcHelper.CreateListField(data.OrderTypes, filter.OrderTypeId, true);
            var administrators = WebMvcHelper.CreateMultiSelectField(data.Administrators, filter.AdministratiorIds);
            var statuses = WebMvcHelper.CreateMultiSelectField(data.OrderStatuses, filter.StatusIds);

            return new OrdersIndexModel(orderTypes, administrators, statuses);
        }
    }
}