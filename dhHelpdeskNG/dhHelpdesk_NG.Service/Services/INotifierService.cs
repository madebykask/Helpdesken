namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;

    public interface INotifierService
    {
        void AddNotifier(NewNotifier notifier);

        void UpdateNotifier(UpdatedNotifier notifier, int customerId);

        void DeleteNotifier(int notifierId);

        void UpdateSettings(FieldSettings settings);

        List<Caption> GetSettingsCaptions(int customerId, int languageId);
    }
}
