namespace DH.Helpdesk.Web.Areas.OrderAccounts.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
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

        private readonly IOrderAccountProxyService orderAccountProxyService;

        public SettingsController(
            IMasterDataService masterDataService,
            IOrderAccountSettingsProxyService orderAccountSettingsProxyService,
            ISettingsViewModelMapper settingsViewModelMapper,
            ISettingsDtoMapper settingsDtoMapper,
            IOrderAccountProxyService orderAccountProxyService)
            : base(masterDataService)
        {
            this.orderAccountSettingsProxyService = orderAccountSettingsProxyService;
            this.settingsViewModelMapper = settingsViewModelMapper;
            this.settingsDtoMapper = settingsDtoMapper;
            this.orderAccountProxyService = orderAccountProxyService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ViewResult Index(int? activityType)
        {
            SettingsIndexModel viewModel;
            int id;
            if (!activityType.HasValue)
            {
                List<int> activities = this.orderAccountProxyService.GetAccountActivivtieIds();

                if (activities == null || !activities.Any())
                {
                    viewModel = new SettingsIndexModel(0, new List<ItemOverview>(), null);
                    return this.View(viewModel);
                }

                id = activities.First();
            }
            else
            {
                id = activityType.Value;
            }

            AccountFieldsSettingsForEditResponse data =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForEditResponse(id, OperationContext);

            HeadersFieldSettings headers = this.orderAccountSettingsProxyService.GetHeadersFieldSettings(id);
            AccountFieldsSettingsModel model = this.settingsViewModelMapper.BuildModel(
                data.AccountFieldsSettingsForEdit,
                headers);

            viewModel = new SettingsIndexModel(id, data.AccountTypes, model);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(int activityType, AccountFieldsSettingsModel accountFieldsSettingsModel)
        {
            AccountFieldsSettingsForUpdate dto = this.settingsDtoMapper.BuildModel(
                activityType,
                accountFieldsSettingsModel,
                this.OperationContext);
            this.orderAccountSettingsProxyService.Update(dto, this.OperationContext);

            this.orderAccountSettingsProxyService.UpdateHeadersFieldSettings(
                activityType,
                new HeadersFieldSettings(
                    accountFieldsSettingsModel.Headers.OrderLabel,
                    accountFieldsSettingsModel.Headers.UserLabel,
                    accountFieldsSettingsModel.Headers.AccountLabel,
                    accountFieldsSettingsModel.Headers.ContactLabel,
                    accountFieldsSettingsModel.Headers.DeliveryLabel,
                    accountFieldsSettingsModel.Headers.ProgramLabel));

            return this.RedirectToAction("Index", new { activityType });
        }
    }
}