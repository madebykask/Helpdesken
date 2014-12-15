namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.Accounts;
    using DH.Helpdesk.Services.Response.Account;

    public class OrderAccountSettingsProxyService : IOrderAccountSettingsProxyService
    {
        private readonly IOrderAccountSettingsService orderAccountSettingsService;

        private readonly IOrderAccountDefaultSettingsCreator accountDefaultSettingsCreator;

        private readonly IOrderAccountService orderAccountService;

        public OrderAccountSettingsProxyService(
            IOrderAccountSettingsService orderAccountSettingsService,
            IOrderAccountDefaultSettingsCreator accountDefaultSettingsCreator,
            IOrderAccountService orderAccountService)
        {
            this.orderAccountSettingsService = orderAccountSettingsService;
            this.accountDefaultSettingsCreator = accountDefaultSettingsCreator;
            this.orderAccountService = orderAccountService;
        }

        public HeadersFieldSettings GetHeadersFieldSettings(int accountActivityId)
        {
            return this.orderAccountSettingsService.GetHeadersFieldSettings(accountActivityId);
        }

        public void UpdateHeadersFieldSettings(int accountActivityId, HeadersFieldSettings dto)
        {
            this.orderAccountSettingsService.UpdateHeadersFieldSettings(accountActivityId, dto);
        }

        public AccountFieldsSettingsForEditResponse GetFieldsSettingsForEditResponse(
            int accountActivityId,
            OperationContext context)
        {
            this.accountDefaultSettingsCreator.Create(accountActivityId, context);

            AccountFieldsSettingsForEdit settings =
                this.orderAccountSettingsService.GetFieldsSettingsForEdit(accountActivityId, context);
            List<ItemOverview> options = this.orderAccountService.GetAccountActivivties();

            var response = new AccountFieldsSettingsForEditResponse(settings, options);

            return response;
        }

        public AccountFieldsSettingsForModelEdit GetFieldsSettingsForModelEdit(
            int accountActivityId,
            OperationContext context)
        {
            this.accountDefaultSettingsCreator.Create(accountActivityId, context);
            return this.orderAccountSettingsService.GetFieldsSettingsForModelEdit(accountActivityId, context);
        }

        public AccountFieldsSettingsOverview GetFieldsSettingsOverview(int accountActivityId, OperationContext context)
        {
            this.accountDefaultSettingsCreator.Create(accountActivityId, context);
            return this.orderAccountSettingsService.GetFieldsSettingsOverview(accountActivityId, context);
        }

        public List<AccountFieldsSettingsOverviewWithActivity> GetFieldsSettingsOverviews(OperationContext context)
        {
            List<int> ids = this.orderAccountService.GetAccountActivivtieIds();

            foreach (int id in ids)
            {
                this.accountDefaultSettingsCreator.Create(id, context);
            }

            return this.orderAccountSettingsService.GetFieldsSettingsOverviews(context);
        }

        public AccountFieldsSettingsForProcessing GetFieldsSettingsForProcessing(
            int accountActivityId,
            OperationContext context)
        {
            this.accountDefaultSettingsCreator.Create(accountActivityId, context);
            return this.orderAccountSettingsService.GetFieldsSettingsForProcessing(accountActivityId, context);
        }

        public void Update(AccountFieldsSettingsForUpdate dto, OperationContext context)
        {
            this.orderAccountSettingsService.Update(dto, context);
        }
    }
}