using DH.Helpdesk.BusinessData.Models.Orders.Index;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.SelfService.Models.Orders;

namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders
{

    public interface IOrdersModelFactory
    {
        OrdersIndexModel GetIndexModel(OrdersFilterData data, OrdersFilterModel filter);

        OrdersGridModel Create(SearchResponse response, SortField sortField, bool showType);
    }
}