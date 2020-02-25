using DH.Helpdesk.BusinessData.Models.Orders.Order;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders.Concrete;
using DH.Helpdesk.SelfService.Models.Orders.OrderEdit;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders
{
    public interface INewOrderModelFactory
    {
        FullOrderEditModel Create(
                        string temporatyId, 
                        NewOrderEditData data,
                        OrderUserData userContext,
                        bool createCaseFromOrder,
                        int customerId,
                        int? orderTypeId,
						IGlobalSettingService globalSettingService);
    }
}