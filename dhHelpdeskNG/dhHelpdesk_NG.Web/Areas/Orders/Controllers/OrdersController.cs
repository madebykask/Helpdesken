namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        private readonly IWorkContext workContext;

        private readonly IOrdersModelFactory ordersModelFactory;

        private readonly TemporaryIdProvider temporaryIdProvider;

        private readonly INewOrderModelFactory newOrderModelFactory;

        private readonly IOrderModelFactory orderModelFactory;

        public OrdersController(
                IMasterDataService masterDataService, 
                IOrdersService ordersService, 
                IWorkContext workContext, 
                IOrdersModelFactory ordersModelFactory, 
                TemporaryIdProvider temporaryIdProvider, 
                INewOrderModelFactory newOrderModelFactory, 
                IOrderModelFactory orderModelFactory)
            : base(masterDataService)
        {
            this.ordersService = ordersService;
            this.workContext = workContext;
            this.ordersModelFactory = ordersModelFactory;
            this.temporaryIdProvider = temporaryIdProvider;
            this.newOrderModelFactory = newOrderModelFactory;
            this.orderModelFactory = orderModelFactory;
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

            var parameters = new SearchParameters(
                                    this.workContext.Customer.CustomerId,
                                    filters.OrderTypeId,
                                    filters.AdministratiorIds,
                                    filters.StartDate,
                                    filters.EndDate,
                                    filters.StatusIds,
                                    filters.Text,
                                    filters.RecordsOnPage,
                                    filters.SortField);

            var response = this.ordersService.Search(parameters);
            var ordersModel = this.ordersModelFactory.Create(response, filters.SortField);

            return this.PartialView(ordersModel);
        }

        [HttpGet]
        public ViewResult Order(int? orderId)
        {
            return this.View();
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult CreateOrder(int orderTypeForCteateOrderId)
        {
            var data = this.ordersService.GetNewOrderEditData(this.workContext.Customer.CustomerId, orderTypeForCteateOrderId);
            var temporaryId = this.temporaryIdProvider.ProvideTemporaryId();

            var model = this.newOrderModelFactory.Create(
                                                temporaryId, 
                                                data,
                                                this.workContext,
                                                orderTypeForCteateOrderId);

            return this.View("New", model);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return this.View();
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(FullOrderEditModel model)
        {
            var id = int.Parse(model.Id);

            return this.RedirectToAction("Edit", new { id });
        }
    }
}
