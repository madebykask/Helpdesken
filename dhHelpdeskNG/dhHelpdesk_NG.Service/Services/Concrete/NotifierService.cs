namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Notifiers;

    public sealed class NotifierService : INotifierService
    {
        private readonly INotifierDynamicRulesValidator notifierValidator;

        private readonly INotifierRepository notifierRepository;

        private readonly INotifierFieldSettingRepository notifierFieldSettingRepository;

        private readonly IRestorer<Notifier, NotifierProcessingSettings> notifierRestorer;

        public NotifierService(
            INotifierRepository notifierRepository,
            INotifierDynamicRulesValidator notifierValidator,
            INotifierFieldSettingRepository notifierFieldSettingRepository,
            IRestorer<Notifier, NotifierProcessingSettings> notifierRestorer)
        {
            this.notifierRepository = notifierRepository;
            this.notifierValidator = notifierValidator;
            this.notifierFieldSettingRepository = notifierFieldSettingRepository;
            this.notifierRestorer = notifierRestorer;
        }

        public void AddNotifier(Notifier notifier)
        {
            var processingSettings = this.notifierFieldSettingRepository.GetProcessingSettings(notifier.CustomerId);
            this.notifierValidator.Validate(notifier, processingSettings);

            this.notifierRepository.AddNotifier(notifier);
            this.notifierRepository.Commit();
        }

        public void UpdateNotifier(Notifier notifier, int customerId)
        {
            var processingSettings = this.notifierFieldSettingRepository.GetProcessingSettings(customerId);
            var existingNotifier = this.notifierRepository.FindExistingNotifierById(notifier.Id);

            this.notifierRestorer.Restore(notifier, existingNotifier, processingSettings);
            this.notifierValidator.Validate(notifier, existingNotifier, processingSettings);

            this.notifierRepository.UpdateNotifier(notifier);
            this.notifierRepository.Commit();
        }

        public void DeleteNotifier(int notifierId)
        {
            this.notifierRepository.DeleteById(notifierId);
            this.notifierRepository.Commit();
        }

        public void UpdateSettings(FieldSettings settings)
        {
            this.notifierFieldSettingRepository.UpdateSettings(settings);
            this.notifierFieldSettingRepository.Commit();
        }
    }
}