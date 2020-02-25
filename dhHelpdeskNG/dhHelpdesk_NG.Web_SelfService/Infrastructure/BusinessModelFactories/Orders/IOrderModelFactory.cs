using DH.Helpdesk.BusinessData.Models.Orders.Order;
using DH.Helpdesk.SelfService.Models.Orders;
using DH.Helpdesk.SelfService.Models.Orders.OrderEdit;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders
{
    public interface IOrderModelFactory
    {
        FullOrderEditModel Create(FindOrderResponse response, int customerId, IGlobalSettingService globalSettingService);
    }
}