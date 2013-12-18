namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Notifiers;

    public interface INotifierFieldSettingRepository : IRepository<ComputerUserFieldSettings>
    {
        FieldDisplayRulesDto FindFieldDisplayRulesByCustomerId(int customerId);

        FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSetting(UpdatedFieldsSettingsDto setting);

        DisplayFieldSettingsDto FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId);
    }
}
