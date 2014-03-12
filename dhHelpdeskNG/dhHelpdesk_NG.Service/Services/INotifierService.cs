namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;

    public interface INotifierService
    {
        void AddNotifier(NewNotifier notifier);

        void UpdateNotifier(UpdatedNotifier notifier, int customerId);

        void DeleteNotifier(int notifierId);

        void UpdateSettings(UpdatedFieldSettings settings);

        List<Caption> GetSettingsCaptions(int customerId, int languageId);
    }
}
