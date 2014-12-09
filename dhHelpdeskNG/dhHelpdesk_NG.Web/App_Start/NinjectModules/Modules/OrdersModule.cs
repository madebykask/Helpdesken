namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete;

    using Ninject.Modules;

    public sealed class OrdersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IOrdersModelFactory>().To<OrdersModelFactory>().InSingletonScope();
            this.Bind<IOrderFieldSettingsModelFactory>().To<OrderFieldSettingsModelFactory>().InSingletonScope();
            this.Bind<IConfigurableFieldModelFactory>().To<ConfigurableFieldModelFactory>().InSingletonScope();
            this.Bind<INewOrderModelFactory>().To<NewOrderModelFactory>().InSingletonScope();
            this.Bind<IOrderModelFactory>().To<OrderModelFactory>().InSingletonScope();
            this.Bind<IUpdateOrderModelFactory>().To<UpdateOrderModelFactory>().InSingletonScope();
        }
    }
}