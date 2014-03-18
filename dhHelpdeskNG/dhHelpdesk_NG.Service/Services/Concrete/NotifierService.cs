namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Services.Restorers.Notifiers;
    using DH.Helpdesk.Services.Validators.Notifier;

    public sealed class NotifierService : INotifierService
    {
        private readonly INotifierDynamicRulesValidator notifierDynamicRulesValidator;

        private readonly INotifierRepository notifierRepository;

        private readonly INotifierFieldSettingRepository notifierFieldSettingRepository;

        private readonly INotifierFieldSettingLanguageRepository notifierFieldSettingLanguageRepository;

        private readonly INotifierRestorer notifierRestorer;

        public NotifierService(
            INotifierRepository notifierRepository,
            INotifierDynamicRulesValidator notifierDynamicRulesValidator,
            INotifierFieldSettingRepository notifierFieldSettingRepository, 
            INotifierFieldSettingLanguageRepository notifierFieldSettingLanguageRepository, 
            INotifierRestorer notifierRestorer)
        {
            this.notifierRepository = notifierRepository;
            this.notifierDynamicRulesValidator = notifierDynamicRulesValidator;
            this.notifierFieldSettingRepository = notifierFieldSettingRepository;
            this.notifierFieldSettingLanguageRepository = notifierFieldSettingLanguageRepository;
            this.notifierRestorer = notifierRestorer;
        }

        public void AddNotifier(Notifier notifier)
        {
            var processingSettings = this.notifierFieldSettingRepository.GetProcessingSettings(notifier.CustomerId);
            this.notifierDynamicRulesValidator.Validate(notifier, processingSettings);

            this.notifierRepository.AddNotifier(notifier);
            this.notifierRepository.Commit();
        }

        public void UpdateNotifier(Notifier notifier, int customerId)
        {
            var processingSettings = this.notifierFieldSettingRepository.GetProcessingSettings(customerId);
            var existingNotifier = this.notifierRepository.FindExistingNotifierById(notifier.Id);
            
            this.notifierRestorer.Restore(notifier, existingNotifier, processingSettings);
            this.notifierDynamicRulesValidator.Validate(notifier, existingNotifier, processingSettings);

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

        public List<Caption> GetSettingsCaptions(int customerId, int languageId)
        {
            return this.notifierFieldSettingLanguageRepository.FindByCustomerIdAndLanguageId(customerId, languageId);
        }
    }
}
