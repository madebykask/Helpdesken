namespace DH.Helpdesk.Services.Infrastructure.BusinessModelValidators.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(Notifier updatedNotifier, Notifier existingNotifier, NotifierProcessingSettings settings);

        void Validate(Notifier newNotifier, NotifierProcessingSettings settings);
    }
}