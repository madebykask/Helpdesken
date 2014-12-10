namespace DH.Helpdesk.Web.Areas.OrderAccounts.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Infrastructure;

    public class OrderController : UserInteractionController
    {
        private const string FilterName = "OrderAccountFilter";

        private readonly IOrderAccountProxyService orderAccountProxyService;

        private readonly IOrderAccountSettingsProxyService orderAccountSettingsProxyService;

        public OrderController(
            IMasterDataService masterDataService,
            IOrderAccountProxyService orderAccountProxyService,
            IOrderAccountSettingsProxyService orderAccountSettingsProxyService)
            : base(masterDataService)
        {
            this.orderAccountProxyService = orderAccountProxyService;
            this.orderAccountSettingsProxyService = orderAccountSettingsProxyService;
        }

        public ViewResult Index(int? acountActivity)
        {
            var currentFilter = SessionFacade.FindPageFilters<Models.Order.Filter>(FilterName)
                                ?? Models.Order.Filter.CreateDefault();

            AccountOverviewResponse data =
                this.orderAccountProxyService.GetOverviewResponse(
                    currentFilter.CreateRequest(acountActivity),
                    OperationContext);

            List<AccountFieldsSettingsOverviewWithActivity> settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsOverviews(OperationContext);

            return this.View();
        }
    }
}