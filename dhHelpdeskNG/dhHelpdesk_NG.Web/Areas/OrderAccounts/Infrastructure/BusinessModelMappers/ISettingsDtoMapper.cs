namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings;

    public interface ISettingsDtoMapper
    {
        AccountFieldsSettingsForUpdate BuildModel(
            int activityType,
            AccountFieldsSettingsModel settings,
            OperationContext operationContext);
    }
}