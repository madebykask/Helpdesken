namespace DH.Helpdesk.Web.Areas.OrderAccounts.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Infrastructure;

    public class OrderController : UserInteractionController
    {
        private const string FilterName = "OrderAccountFilter";

        private readonly IOrderAccountProxyService orderAccountProxyService;

        private readonly IOrderAccountSettingsProxyService orderAccountSettingsProxyService;

        private readonly IUserService userService;

        public OrderController(
            IMasterDataService masterDataService,
            IOrderAccountProxyService orderAccountProxyService,
            IOrderAccountSettingsProxyService orderAccountSettingsProxyService,
            IUserService userService)
            : base(masterDataService)
        {
            this.orderAccountProxyService = orderAccountProxyService;
            this.orderAccountSettingsProxyService = orderAccountSettingsProxyService;
            this.userService = userService;
        }

        public ViewResult Index(int? activityType)
        {
            var currentFilter = SessionFacade.FindPageFilters<Models.Order.Filter>(FilterName)
                                ?? Models.Order.Filter.CreateDefault();

            List<ItemOverview> activities = this.orderAccountProxyService.GetAccountActivities();
            List<ItemOverview> users = this.userService.FindActiveOverviews(OperationContext.CustomerId);

            OrdersIndexModel viewModel = OrdersIndexModel.BuildViewModel(activityType, activities, users, currentFilter);

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Grid(Models.Order.Filter filter, int? activityType)
        {
            SessionFacade.SavePageFilters(FilterName, filter);


            List<AccountOverview> models = this.orderAccountProxyService.GetOverviews(
                filter.CreateRequest(activityType),
                OperationContext);

            List<AccountFieldsSettingsOverviewWithActivity> settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsOverviews(OperationContext);

            List<GridModel> gridModels = GridModel.BuildGrid(models, settings);

            return this.PartialView("Grids", gridModels);
        }
    }
}