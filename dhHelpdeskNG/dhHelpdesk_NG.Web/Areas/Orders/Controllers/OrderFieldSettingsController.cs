namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class OrderFieldSettingsController : BaseController
    {
        private readonly IOrderFieldSettingsService orderFieldSettingsService;

        private readonly IOrderFieldSettingsModelFactory orderFieldSettingsModelFactory;

        private readonly IWorkContext workContext;

        public OrderFieldSettingsController(
                IMasterDataService masterDataService, 
                IOrderFieldSettingsService orderFieldSettingsService, 
                IWorkContext workContext, 
                IOrderFieldSettingsModelFactory orderFieldSettingsModelFactory)
            : base(masterDataService)
        {
            this.orderFieldSettingsService = orderFieldSettingsService;
            this.workContext = workContext;
            this.orderFieldSettingsModelFactory = orderFieldSettingsModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var filters = SessionFacade.FindPageFilters<OrderFieldSettingsFilterModel>(PageName.OrdersOrderFieldSettings);
            if (filters == null)
            {
                filters = OrderFieldSettingsFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.OrdersOrderFieldSettings, filters);
            }

            var data = this.orderFieldSettingsService.GetFilterData(this.workContext.Customer.CustomerId);
            var model = this.orderFieldSettingsModelFactory.GetIndexModel(data, filters);

            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult OrderFieldSettings(OrderFieldSettingsIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<OrderFieldSettingsFilterModel>(PageName.OrdersOrderFieldSettings);

            SessionFacade.SavePageFilters(PageName.OrdersOrderFieldSettings, filters);

            var parameters = new SearchParameters(filters.OrderTypeId);

            return this.PartialView();
        }
    }
}
