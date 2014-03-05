namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Services.EntitiesRestorers.Notifiers;
    using DH.Helpdesk.Services.Validators;
    using DH.Helpdesk.Services.Validators.Common;
    using DH.Helpdesk.Services.Validators.Notifier;

    public sealed class NotifierService : INotifierService
    {
        private readonly INotifierDynamicRulesValidator notifierDynamicRulesValidator;

        private readonly INotifierRepository notifierRepository;

        private readonly INotifierFieldSettingRepository notifierFieldSettingRepository;

        public NotifierService(
            INotifierRepository notifierRepository,
            INotifierDynamicRulesValidator notifierDynamicRulesValidator,
            INotifierFieldSettingRepository notifierFieldSettingRepository)
        {
            this.notifierRepository = notifierRepository;
            this.notifierDynamicRulesValidator = notifierDynamicRulesValidator;
            this.notifierFieldSettingRepository = notifierFieldSettingRepository;
        }

        public void AddNotifier(NewNotifierDto notifier)
        {
            var validationSettings = this.LoadValidationSettings(notifier.CustomerId);
            this.notifierDynamicRulesValidator.Validate(notifier, validationSettings);
            this.notifierRepository.AddNotifier(notifier);
            this.notifierRepository.Commit();
        }

        public void UpdateNotifier(UpdatedNotifierDto notifier, int customerId)
        {
            var existingNotifier = this.notifierRepository.FindExistingNotifierById(notifier.Id);
            var displayRules = this.notifierFieldSettingRepository.FindFieldDisplayRulesByCustomerId(customerId);
            NotifierRestorer.Restore(notifier, existingNotifier, displayRules);
            var validationSettings = this.LoadValidationSettings(customerId);
            this.notifierDynamicRulesValidator.Validate(notifier, existingNotifier, validationSettings);
            this.notifierRepository.UpdateNotifier(notifier);
            this.notifierRepository.Commit();
        }

        private FieldValidationSettings LoadValidationSettings(int customerId)
        {
            var displayRules = this.notifierFieldSettingRepository.FindFieldDisplayRulesByCustomerId(customerId);

            return
                new FieldValidationSettings(
                    new ElementaryValidationRule(!displayRules.Domain.Show, displayRules.Domain.Required),
                    new ElementaryValidationRule(!displayRules.LoginName.Show, displayRules.LoginName.Required),
                    new ElementaryValidationRule(!displayRules.FirstName.Show, displayRules.FirstName.Required),
                    new ElementaryValidationRule(!displayRules.Initials.Show, displayRules.Initials.Required),
                    new ElementaryValidationRule(!displayRules.LastName.Show, displayRules.LastName.Required),
                    new ElementaryValidationRule(!displayRules.DisplayName.Show, displayRules.DisplayName.Required),
                    new ElementaryValidationRule(!displayRules.Place.Show, displayRules.Place.Required),
                    new ElementaryValidationRule(!displayRules.Phone.Show, displayRules.Phone.Required),
                    new ElementaryValidationRule(!displayRules.CellPhone.Show, displayRules.CellPhone.Required),
                    new ElementaryValidationRule(!displayRules.Email.Show, displayRules.Email.Required),
                    new ElementaryValidationRule(!displayRules.Code.Show, displayRules.Code.Required),
                    new ElementaryValidationRule(!displayRules.PostalAddress.Show, displayRules.PostalAddress.Required),
                    new ElementaryValidationRule(!displayRules.PostalCode.Show, displayRules.PostalCode.Required),
                    new ElementaryValidationRule(!displayRules.City.Show, displayRules.City.Required),
                    new ElementaryValidationRule(!displayRules.Title.Show, displayRules.Title.Required),
                    new ElementaryValidationRule(!displayRules.Department.Show, displayRules.Department.Required),
                    new ElementaryValidationRule(!displayRules.Unit.Show, displayRules.Unit.Required),
                    new ElementaryValidationRule(
                        !displayRules.OrganizationUnit.Show, displayRules.OrganizationUnit.Required),
                    new ElementaryValidationRule(!displayRules.Division.Show, displayRules.Division.Required),
                    new ElementaryValidationRule(!displayRules.Manager.Show, displayRules.Manager.Required),
                    new ElementaryValidationRule(!displayRules.Group.Show, displayRules.Group.Required),
                    new ElementaryValidationRule(!displayRules.Password.Show, displayRules.Password.Required),
                    new ElementaryValidationRule(!displayRules.Other.Show, displayRules.Other.Required),
                    new ElementaryValidationRule(!displayRules.Ordered.Show, displayRules.Ordered.Required));
        }
    }
}
