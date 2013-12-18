namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Notifiers;

    public interface INotifierFieldsSettingsRepository : IRepository<ComputerUserFieldSettings>
    {
        FieldValidSettings FindValidSettingsBy(int customerId);

        FieldsSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSetting(UpdatedFieldsSettingsDto setting);

        DisplayFieldsSettingsDto FindDisplayFieldsSettingsByCustomerIdAndLanguageId(int customerId, int languageId);
    }
}
