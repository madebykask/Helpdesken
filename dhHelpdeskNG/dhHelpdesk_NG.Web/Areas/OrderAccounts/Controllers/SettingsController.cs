namespace DH.Helpdesk.Web.Areas.OrderAccounts.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class SettingsController : UserInteractionController
    {
        private readonly IOrderAccountSettingsProxyService orderAccountSettingsProxyService;

        private readonly ISettingsViewModelMapper settingsViewModelMapper;

        private readonly ISettingsDtoMapper settingsDtoMapper;

        public SettingsController(
            IMasterDataService masterDataService,
            IOrderAccountSettingsProxyService orderAccountSettingsProxyService,
            ISettingsViewModelMapper settingsViewModelMapper,
            ISettingsDtoMapper settingsDtoMapper)
            : base(masterDataService)
        {
            this.orderAccountSettingsProxyService = orderAccountSettingsProxyService;
            this.settingsViewModelMapper = settingsViewModelMapper;
            this.settingsDtoMapper = settingsDtoMapper;
        }

        [HttpGet]
        public ViewResult Index(int activityType)
        {
            AccountFieldsSettingsForEditResponse data =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForEditResponse(activityType, OperationContext);

            AccountFieldsSettingsModel model = this.settingsViewModelMapper.BuildModel(
                data.AccountFieldsSettingsForEdit);

            var viewModel = new SettingsIndexModel(activityType, data.AccountTypes, model);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(int activityType, AccountFieldsSettingsModel model)
        {
            AccountFieldsSettingsForUpdate dto = this.settingsDtoMapper.BuildModel(
                activityType,
                model,
                this.OperationContext);
            this.orderAccountSettingsProxyService.Update(dto, this.OperationContext);

            return this.RedirectToAction("Index", new { activityType });
        }
    }
}