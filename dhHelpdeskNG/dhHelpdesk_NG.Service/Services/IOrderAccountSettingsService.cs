namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;

    public interface IOrderAccountSettingsService
    {
        HeadersFieldSettings GetHeadersFieldSettings(int accountActivityId);

        void UpdateHeadersFieldSettings(int accountActivityId, HeadersFieldSettings dto);

        AccountFieldsSettingsForEdit GetFieldsSettingsForEdit(int accountActivityId, OperationContext context);

        AccountFieldsSettingsForModelEdit GetFieldsSettingsForModelEdit(int accountActivityId, OperationContext context);

        AccountFieldsSettingsOverview GetFieldsSettingsOverview(int accountActivityId, OperationContext context);

        List<AccountFieldsSettingsOverviewWithActivity> GetFieldsSettingsOverviews(OperationContext context);

        AccountFieldsSettingsForProcessing GetFieldsSettingsForProcessing(
            int accountActivityId,
            OperationContext context);

        void Update(AccountFieldsSettingsForUpdate dto, OperationContext context);
    }
}