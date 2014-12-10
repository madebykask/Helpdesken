namespace DH.Helpdesk.Web.Areas.OrderAccounts.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class OrderController : UserInteractionController
    {
        private const string FilterName = "OrderAccountFilter";

        private readonly IOrderAccountProxyService orderAccountProxyService;

        private readonly IOrderAccountSettingsProxyService orderAccountSettingsProxyService;

        private readonly IUserService userService;

        private readonly IOrganizationService organizationService;

        private readonly IOrderModelMapper orderModelMapper;

        private readonly IAccountDtoMapper accountDtoMapper;

        public OrderController(
            IMasterDataService masterDataService,
            IOrderAccountProxyService orderAccountProxyService,
            IOrderAccountSettingsProxyService orderAccountSettingsProxyService,
            IUserService userService,
            IOrganizationService organizationService,
            IOrderModelMapper orderModelMapper,
            IAccountDtoMapper accountDtoMapper)
            : base(masterDataService)
        {
            this.orderAccountProxyService = orderAccountProxyService;
            this.orderAccountSettingsProxyService = orderAccountSettingsProxyService;
            this.userService = userService;
            this.organizationService = organizationService;
            this.orderModelMapper = orderModelMapper;
            this.accountDtoMapper = accountDtoMapper;
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

        [HttpGet]
        public ViewResult Edit(int id, int activityType)
        {
            AccountForEdit model = this.orderAccountProxyService.Get(id);
            AccountFieldsSettingsForModelEdit settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForModelEdit(activityType, OperationContext);
            AccountOptionsResponse options = this.orderAccountProxyService.GetOptions(activityType, OperationContext);

            AccountModel viewModel = this.orderModelMapper.BuildViewModel(model, options, settings);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult Edit(AccountModel model)
        {
            AccountForUpdate dto = this.accountDtoMapper.BuidForUpdate(model, null, OperationContext); // todo
            this.orderAccountProxyService.Update(dto, this.OperationContext);

            return this.View("Edit", new { id = dto.Id, activityType = dto.ActivityId });
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult EditAndClose(AccountModel model)
        {
            AccountForUpdate dto = this.accountDtoMapper.BuidForUpdate(model, null, OperationContext); // todo
            this.orderAccountProxyService.Update(dto, this.OperationContext);

            return this.View("Index", new { activityType = dto.ActivityId });
        }

        [HttpGet]
        public ViewResult New(int activityType)
        {
            AccountFieldsSettingsForModelEdit settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForModelEdit(activityType, OperationContext);
            AccountOptionsResponse options = this.orderAccountProxyService.GetOptions(activityType, OperationContext);
            IdAndNameOverview activity = this.orderAccountProxyService.GetAccountActivityItemOverview(activityType);

            AccountModel viewModel = this.orderModelMapper.BuildViewModel(activityType, options, settings);
            viewModel.ActivityName = activity.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult New(AccountModel model)
        {
            AccountForInsert dto = this.accountDtoMapper.BuidForInsert(model, null, OperationContext); // todo
            int id = this.orderAccountProxyService.Add(dto, this.OperationContext);

            return this.View("Edit", new { id, activityType = dto.ActivityId });
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult NewAndClose(AccountModel model)
        {
            AccountForInsert dto = this.accountDtoMapper.BuidForInsert(model, null, OperationContext); // todo
            int id = this.orderAccountProxyService.Add(dto, this.OperationContext);

            return this.View("Index", new { activityType = dto.ActivityId });
        }
    }
}