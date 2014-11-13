namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        private readonly IOrderFieldSettingsService orderFieldSettingsService;

        private readonly IWorkContext workContext;

        private readonly IOrdersModelFactory ordersModelFactory;

        public OrdersController(
                IMasterDataService masterDataService, 
                IOrdersService ordersService, 
                IOrderFieldSettingsService orderFieldSettingsService, 
                IWorkContext workContext, 
                IOrdersModelFactory ordersModelFactory)
            : base(masterDataService)
        {
            this.ordersService = ordersService;
            this.orderFieldSettingsService = orderFieldSettingsService;
            this.workContext = workContext;
            this.ordersModelFactory = ordersModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var filters = SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.OrdersOrders);
            if (filters == null)
            {
                filters = OrdersFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.OrdersOrders, filters);
            }

            var data = this.ordersService.GetOrdersFilterData(this.workContext.Customer.CustomerId);

            var model = this.ordersModelFactory.GetIndexModel(data, filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Orders(OrdersIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.OrdersOrders);

            SessionFacade.SavePageFilters(PageName.OrdersOrders, filters);

            return this.PartialView();
        }

        [HttpGet]
        public ViewResult Order(int? ordersId)
        {
            return this.View();
        }
    }
}
