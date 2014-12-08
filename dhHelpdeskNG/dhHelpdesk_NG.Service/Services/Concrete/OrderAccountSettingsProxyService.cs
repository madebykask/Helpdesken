namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.Services.BusinessLogic.Accounts;

    using Ninject;

    public class OrderAccountSettingsProxyService : IOrderAccountSettingsService
    {
        private readonly IOrderAccountSettingsService orderAccountSettingsService;

        private readonly IOrderAccountDefaultSettingsCreator accountDefaultSettingsCreator;

        public OrderAccountSettingsProxyService(
            [Named("OrderAccountSettingsService")] IOrderAccountSettingsService orderAccountSettingsService,
            IOrderAccountDefaultSettingsCreator accountDefaultSettingsCreator)
        {
            this.orderAccountSettingsService = orderAccountSettingsService;
            this.accountDefaultSettingsCreator = accountDefaultSettingsCreator;
        }

        public AccountFieldsSettingsForEdit GetFieldsSettingsForEdit(int accountActivityId, OperationContext context)
        {
            this.accountDefaultSettingsCreator.Create(accountActivityId, context);
            return this.orderAccountSettingsService.GetFieldsSettingsForEdit(accountActivityId, context);
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