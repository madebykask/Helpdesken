using DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders;
using DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders.Concrete;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders.Concrete;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders.Concrete;
using DH.Helpdesk.Services.BusinessLogic.Orders;
using DH.Helpdesk.Services.BusinessLogic.Orders.Concrete;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete.Orders;
using DH.Helpdesk.Services.Services.Orders;
using Ninject.Modules;

namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    public class OrdersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IOrdersModelFactory>().To<OrdersModelFactory>().InSingletonScope();
            //Bind<IOrderFieldSettingsModelFactory>().To<OrderFieldSettingsModelFactory>().InSingletonScope();
            Bind<IConfigurableFieldModelFactory>().To<ConfigurableFieldModelFactory>().InSingletonScope();
            Bind<IOrdersService>().To<OrdersService>();
            Bind<IOrderRestorer>().To<OrderRestorer>().InSingletonScope();
            Bind<IUpdateOrderRequestValidator>().To<UpdateOrderRequestValidator>().InSingletonScope();
            Bind<IOrdersLogic>().To<OrdersLogic>().InSingletonScope();
            Bind<IHistoriesComparator>().To<HistoriesComparator>().InSingletonScope();
            Bind<INewOrderModelFactory>().To<NewOrderModelFactory>().InSingletonScope();
            Bind<IUpdateOrderModelFactory>().To<UpdateOrderModelFactory>().InSingletonScope();
            Bind<IOrderModelFactory>().To<OrderModelFactory>().InSingletonScope();
            Bind<IOrderFieldSettingsService>().To<OrderFieldSettingsService>();
            Bind<IOrderTypeService>().To<OrderTypeService>();
        }
    }
}
