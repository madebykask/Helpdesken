namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Orders;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders.Concrete;
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
            this.Bind<IOrderRestorer>().To<OrderRestorer>().InSingletonScope();
            this.Bind<ILogsModelFactory>().To<LogsModelFactory>().InSingletonScope();
            this.Bind<IUpdateOrderRequestValidator>().To<UpdateOrderRequestValidator>().InSingletonScope();
            this.Bind<IHistoryModelFactory>().To<HistoryModelFactory>().InSingletonScope();

            this.Bind<IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>>().To<InformReceiverAuditor>().InSingletonScope();
            this.Bind<IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>>().To<InformOrdererAuditor>().InSingletonScope();
            this.Bind<IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>>().To<CreateCaseAuditor>().InSingletonScope();
        }
    }
}