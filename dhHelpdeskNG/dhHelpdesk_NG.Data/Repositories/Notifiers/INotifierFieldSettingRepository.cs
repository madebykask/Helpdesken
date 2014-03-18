namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public interface INotifierFieldSettingRepository : IRepository<ComputerUserFieldSettings>
    {
        FieldDisplayRules FindFieldDisplayRulesByCustomerId(int customerId);

        FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSettings(FieldSettings settings);

        DisplayFieldSettings FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId);
    }
}
