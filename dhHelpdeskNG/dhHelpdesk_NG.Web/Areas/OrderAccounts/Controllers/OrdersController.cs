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

    public class OrdersController : UserInteractionController
    {
        private const string FilterName = "OrderAccountFilter";

        private readonly IOrderAccountProxyService orderAccountProxyService;

        private readonly IOrderAccountSettingsProxyService orderAccountSettingsProxyService;

        private readonly IUserService userService;

        private readonly IOrganizationService organizationService;

        private readonly IOrderModelMapper orderModelMapper;

        private readonly IAccountDtoMapper accountDtoMapper;

        public OrdersController(
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

            var settings = new List<AccountFieldsSettingsOverviewWithActivity>();

            if (activityType.HasValue)
            {
                AccountFieldsSettingsOverview setting =
                    this.orderAccountSettingsProxyService.GetFieldsSettingsOverview(
                        activityType.Value,
                        this.OperationContext);
                var settingOverview = new AccountFieldsSettingsOverviewWithActivity(activityType.Value, setting);

                settings.Add(settingOverview);
            }
            else
            {
                settings = this.orderAccountSettingsProxyService.GetFieldsSettingsOverviews(this.OperationContext);
            }

            List<GridModel> gridModels = GridModel.BuildGrid(models, settings, filter.SortField);

            return this.PartialView("Grids", gridModels);
        }

        [HttpGet]
        public ViewResult Edit(int id, int activityType)
        {
            AccountForEdit model = this.orderAccountProxyService.Get(id);
            AccountFieldsSettingsForModelEdit settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForModelEdit(activityType, OperationContext);
            AccountOptionsResponse options = this.orderAccountProxyService.GetOptions(activityType, OperationContext);
            HeadersFieldSettings headers = this.orderAccountSettingsProxyService.GetHeadersFieldSettings(activityType);

            AccountModel viewModel = this.orderModelMapper.BuildViewModel(model, options, settings, headers);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(AccountModel model, string clickedButton)
        {
            AccountForUpdate dto = this.accountDtoMapper.BuidForUpdate(model, null, OperationContext); // todo
            this.orderAccountProxyService.Update(dto, this.OperationContext);

            if (clickedButton == ClickedButton.Save)
            {
                return this.RedirectToAction("Edit", new { dto.Id, activityType = dto.ActivityId });
            }

            return this.RedirectToAction("Index", new { activityType = dto.ActivityId });
        }

        [HttpPost]
        public RedirectToRouteResult RedirectToNew(int activityTypeForEdit)
        {
            return this.RedirectToAction("New", new { activityTypeForEdit });
        }

        [HttpGet]
        public ViewResult New(int activityTypeForEdit)
        {
            AccountFieldsSettingsForModelEdit settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForModelEdit(
                    activityTypeForEdit,
                    OperationContext);
            AccountOptionsResponse options = this.orderAccountProxyService.GetOptions(
                activityTypeForEdit,
                OperationContext);
            IdAndNameOverview activity =
                this.orderAccountProxyService.GetAccountActivityItemOverview(activityTypeForEdit);

            HeadersFieldSettings headers = this.orderAccountSettingsProxyService.GetHeadersFieldSettings(activityTypeForEdit);

            AccountModel viewModel = this.orderModelMapper.BuildViewModel(
                activityTypeForEdit,
                options,
                settings,
                SessionFacade.CurrentUser,
                headers);
            viewModel.ActivityName = activity.Name;

            return this.View("New", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(AccountModel model, string clickedButton)
        {
            AccountForInsert dto = this.accountDtoMapper.BuidForInsert(model, null, OperationContext); // todo
            int id = this.orderAccountProxyService.Add(dto, this.OperationContext);

            if (clickedButton == ClickedButton.Save)
            {
                return this.RedirectToAction("Edit", new { id, activityType = dto.ActivityId });
            }

            return this.RedirectToAction("Index", new { activityType = dto.ActivityId });
        }

        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            this.orderAccountProxyService.Delete(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult SearchDepartmentsByRegionId(int? selected)
        {
            List<ItemOverview> models = this.organizationService.GetDepartments(
                SessionFacade.CurrentCustomer.Id,
                selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}