namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class OrderFieldSettingsController : BaseController
    {
        private readonly IOrderFieldSettingsService _orderFieldSettingsService;

        private readonly IOrderFieldSettingsModelFactory _orderFieldSettingsModelFactory;

        private readonly IWorkContext _workContext;

        public OrderFieldSettingsController(
                IMasterDataService masterDataService, 
                IOrderFieldSettingsService orderFieldSettingsService, 
                IWorkContext workContext, 
                IOrderFieldSettingsModelFactory orderFieldSettingsModelFactory)
            : base(masterDataService)
        {
            _orderFieldSettingsService = orderFieldSettingsService;
            _workContext = workContext;
            _orderFieldSettingsModelFactory = orderFieldSettingsModelFactory;
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

            var data = this._orderFieldSettingsService.GetFilterData(this._workContext.Customer.CustomerId);
            var model = this._orderFieldSettingsModelFactory.GetIndexModel(data, filters);

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

            var response = this._orderFieldSettingsService.GetOrderFieldSettings(
                                    this._workContext.Customer.CustomerId,
                                    filters.OrderTypeId);
            var settingsModel = this._orderFieldSettingsModelFactory.Create(response, filters.OrderTypeId);

            return this.PartialView(settingsModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult SaveSettings(FullFieldSettingsModel model, int? orderTypeId)
        {
            var customerId = this._workContext.Customer.CustomerId;
            var settings = this._orderFieldSettingsModelFactory.CreateForUpdate(
                                    model,
                                    customerId,
                                    orderTypeId,
                                    DateTime.Now);

            this._orderFieldSettingsService.UpdateSettings(settings);

            return this.RedirectToAction("Index");
        }
    }
}
