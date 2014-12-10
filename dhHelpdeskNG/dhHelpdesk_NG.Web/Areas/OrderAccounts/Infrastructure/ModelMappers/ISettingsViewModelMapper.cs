namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers
{
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings;

    public interface ISettingsViewModelMapper
    {
        AccountFieldsSettingsModel BuildModel(AccountFieldsSettingsForEdit settings);
    }
}