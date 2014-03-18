namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public interface INotifierFieldSettingRepository : IRepository<ComputerUserFieldSettings>
    {
        NotifierProcessingSettings GetProcessingSettings(int customerId);

        FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSettings(FieldSettings settings);

        NotifierOverviewSettings FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId);
    }
}
