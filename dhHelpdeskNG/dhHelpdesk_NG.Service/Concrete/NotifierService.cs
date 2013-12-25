namespace dhHelpdesk_NG.Service.Concrete
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Service.Converters.Notifiers;
    using dhHelpdesk_NG.Service.Validators;
    using dhHelpdesk_NG.Service.Validators.Notifier;
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;

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

        public void AddNotifier(NewNotifier notifier)
        {
            var validationSettings = this.LoadValidationSettings(notifier.CustomerId);
            this.notifierDynamicRulesValidator.Validate(notifier, validationSettings);
            var newNotifier = NewNotifierToNewNotifierDtoConverter.Convert(notifier);
            this.notifierRepository.AddNotifier(newNotifier);
            this.notifierRepository.Commit();
        }

        public void UpdateNotifier(UpdatedNotifier notifier, int customerId)
        {
            var existingNotifier = this.notifierRepository.FindExistingNotifierById(notifier.Id);
            var validationSettings = this.LoadValidationSettings(customerId);
            this.notifierDynamicRulesValidator.Validate(notifier, existingNotifier, validationSettings);
            var updatedNotifier = UpdatedNotifierToUpdatedNotifierDtoConverter.Convert(notifier);
            this.notifierRepository.UpdateNotifier(updatedNotifier);
            this.notifierRepository.Commit();
        }

        private FieldValidationSettings LoadValidationSettings(int customerId)
        {
            var displayRules = this.notifierFieldSettingRepository.FindFieldDisplayRulesByCustomerId(customerId);

            return
                new FieldValidationSettings(
                    new FieldValidationSetting(!displayRules.Domain.Show, displayRules.Domain.Required),
                    new FieldValidationSetting(!displayRules.LoginName.Show, displayRules.LoginName.Required),
                    new FieldValidationSetting(!displayRules.FirstName.Show, displayRules.FirstName.Required),
                    new FieldValidationSetting(!displayRules.Initials.Show, displayRules.Initials.Required),
                    new FieldValidationSetting(!displayRules.LastName.Show, displayRules.LastName.Required),
                    new FieldValidationSetting(!displayRules.DisplayName.Show, displayRules.DisplayName.Required),
                    new FieldValidationSetting(!displayRules.Place.Show, displayRules.Place.Required),
                    new FieldValidationSetting(!displayRules.Phone.Show, displayRules.Phone.Required),
                    new FieldValidationSetting(!displayRules.CellPhone.Show, displayRules.CellPhone.Required),
                    new FieldValidationSetting(!displayRules.Email.Show, displayRules.Email.Required),
                    new FieldValidationSetting(!displayRules.Code.Show, displayRules.Code.Required),
                    new FieldValidationSetting(!displayRules.PostalAddress.Show, displayRules.PostalAddress.Required),
                    new FieldValidationSetting(!displayRules.PostalCode.Show, displayRules.PostalCode.Required),
                    new FieldValidationSetting(!displayRules.City.Show, displayRules.City.Required),
                    new FieldValidationSetting(!displayRules.Title.Show, displayRules.Title.Required),
                    new FieldValidationSetting(!displayRules.Department.Show, displayRules.Department.Required),
                    new FieldValidationSetting(!displayRules.Unit.Show, displayRules.Unit.Required),
                    new FieldValidationSetting(
                        !displayRules.OrganizationUnit.Show, displayRules.OrganizationUnit.Required),
                    new FieldValidationSetting(!displayRules.Division.Show, displayRules.Division.Required),
                    new FieldValidationSetting(!displayRules.Manager.Show, displayRules.Manager.Required),
                    new FieldValidationSetting(!displayRules.Group.Show, displayRules.Group.Required),
                    new FieldValidationSetting(!displayRules.Password.Show, displayRules.Password.Required),
                    new FieldValidationSetting(!displayRules.Other.Show, displayRules.Other.Required),
                    new FieldValidationSetting(!displayRules.Ordered.Show, displayRules.Ordered.Required));
        }
    }
}
