namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;

    public interface IOrderAccountSettingsService
    {
        AccountFieldsSettingsForEdit GetFieldsSettingsForEdit(int accountActivityId, OperationContext context);

        AccountFieldsSettingsForModelEdit GetFieldsSettingsForModelEdit(int accountActivityId, OperationContext context);

        AccountFieldsSettingsOverview GetFieldsSettingsOverview(int accountActivityId, OperationContext context);

        AccountFieldsSettingsForProcessing GetFieldsSettingsForProcessing(
            int accountActivityId,
            OperationContext context);

        void Update(AccountFieldsSettingsForUpdate dto, OperationContext context);
    }
}