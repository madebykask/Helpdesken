namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Notifiers;

    public interface INotifierFieldSettingRepository : IRepository<ComputerUserFieldSettings>
    {
        FieldDisplayRulesDto FindFieldDisplayRulesByCustomerId(int customerId);

        FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSettings(UpdatedFieldSettingsDto fieldSettings);

        DisplayFieldSettingsDto FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId);
    }
}
