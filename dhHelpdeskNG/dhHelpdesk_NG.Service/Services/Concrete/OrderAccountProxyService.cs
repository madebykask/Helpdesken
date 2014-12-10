namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Account;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Accounts;
    using DH.Helpdesk.Services.Requests.Account;
    using DH.Helpdesk.Services.Response.Account;

    public class OrderAccountProxyService : IOrderAccountProxyService
    {
        private readonly IOrderAccountService orderAccountService;

        private readonly IOrderAccountSettingsService orderAccountSettingsService;

        private readonly IAccountValidator accountValidator;

        private readonly IAccountRestorer accountRestorer;

        public OrderAccountProxyService(
            IOrderAccountService orderAccountService,
            IOrderAccountSettingsService orderAccountSettingsService,
            IAccountValidator accountValidator,
            IAccountRestorer accountRestorer)
        {
            this.orderAccountService = orderAccountService;
            this.orderAccountSettingsService = orderAccountSettingsService;
            this.accountValidator = accountValidator;
            this.accountRestorer = accountRestorer;
        }

        public AccountOverviewResponse GetOverviewResponse(AccountFilter filter, OperationContext context)
        {
            List<AccountOverview> overviews = this.orderAccountService.GetOverviews(filter, context);
            List<ItemOverview> activities = this.orderAccountService.GetAccountActivivties();

            var viewModel = new AccountOverviewResponse(overviews, activities);

            return viewModel;
        }

        public AccountForEdit Get(int id)
        {
            return this.orderAccountService.Get(id);
        }

        public void Update(AccountForUpdate dto, OperationContext context)
        {
            AccountForEdit entity = this.orderAccountService.Get(dto.Id);
            AccountFieldsSettingsForProcessing settings =
                this.orderAccountSettingsService.GetFieldsSettingsForProcessing(dto.ActivityId, context);

            this.accountRestorer.Restore(dto, entity, settings);
            this.orderAccountService.Update(dto, context);
        }

        public void Add(AccountForInsert dto, OperationContext context)
        {
            AccountFieldsSettingsForProcessing settings =
                this.orderAccountSettingsService.GetFieldsSettingsForProcessing(dto.ActivityId, context);

            this.accountValidator.Validate(dto, settings);
            this.orderAccountService.Add(dto, context);
        }

        public void Delete(int id)
        {
            this.orderAccountService.Delete(id);
        }
    }
}