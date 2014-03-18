namespace DH.Helpdesk.Services.Restorers.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;

    public interface INotifierRestorer
    {
        void Restore(Notifier notifier, Notifier existingNotifier, NotifierProcessingSettings settings);
    }
}