namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;

    public interface INotifierService
    {
        void AddNotifier(Notifier notifier);

        void UpdateNotifier(Notifier notifier, int customerId);

        void DeleteNotifier(int notifierId);

        void UpdateSettings(FieldSettings settings);

        List<Caption> GetSettingsCaptions(int customerId, int languageId);
    }
}
