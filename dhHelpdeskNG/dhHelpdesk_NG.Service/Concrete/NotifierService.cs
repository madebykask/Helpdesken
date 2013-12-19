namespace dhHelpdesk_NG.Service.Concrete
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Service.Validators;
    using dhHelpdesk_NG.Service.Validators.Notifier;

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
                    new FieldValidationSetting(!displayRules.Domain.Show, displayRules.Domain.Required),
                    new StringFieldValidationSetting(!displayRules.LoginName.Show, displayRules.LoginName.Required, displayRules.LoginName.MinLength),
                    new StringFieldValidationSetting(!displayRules.FirstName.Show, displayRules.FirstName.Required, displayRules.FirstName.MinLength),
                    new StringFieldValidationSetting(!displayRules.Initials.Show, displayRules.Initials.Required, displayRules.Initials.MinLength),
                    new StringFieldValidationSetting(!displayRules.LastName.Show, displayRules.LastName.Required, displayRules.LastName.MinLength),
                    new StringFieldValidationSetting(!displayRules.DisplayName.Show, displayRules.DisplayName.Required, displayRules.DisplayName.MinLength),
                    new StringFieldValidationSetting(!displayRules.Place.Show, displayRules.Place.Required, displayRules.Place.MinLength),
                    new StringFieldValidationSetting(!displayRules.Phone.Show, displayRules.Phone.Required, displayRules.Phone.MinLength),
                    new StringFieldValidationSetting(!displayRules.CellPhone.Show, displayRules.CellPhone.Required, displayRules.CellPhone.MinLength),
                    new StringFieldValidationSetting(!displayRules.Email.Show, displayRules.Email.Required, displayRules.Email.MinLength),
                    new StringFieldValidationSetting(!displayRules.Code.Show, displayRules.Code.Required, displayRules.Code.MinLength),
                    new StringFieldValidationSetting(
                        !displayRules.PostalAddress.Show, displayRules.PostalAddress.Required, displayRules.PostalAddress.MinLength),
                    new StringFieldValidationSetting(!displayRules.PostalCode.Show, displayRules.PostalCode.Required, displayRules.PostalCode.MinLength),
                    new StringFieldValidationSetting(!displayRules.City.Show, displayRules.City.Required, displayRules.City.MinLength),
                    new StringFieldValidationSetting(!displayRules.Title.Show, displayRules.Title.Required, displayRules.Title.MinLength),
                    new FieldValidationSetting(!displayRules.Department.Show, displayRules.Department.Required),
                    new StringFieldValidationSetting(!displayRules.Unit.Show, displayRules.Unit.Required, displayRules.Unit.MinLength),
                    new FieldValidationSetting(
                        !displayRules.OrganizationUnit.Show, displayRules.OrganizationUnit.Required),
                    new FieldValidationSetting(!displayRules.Division.Show, displayRules.Division.Required),
                    new FieldValidationSetting(!displayRules.Manager.Show, displayRules.Manager.Required),
                    new FieldValidationSetting(!displayRules.Group.Show, displayRules.Group.Required),
                    new StringFieldValidationSetting(!displayRules.Password.Show, displayRules.Password.Required, displayRules.Password.MinLength),
                    new StringFieldValidationSetting(!displayRules.Other.Show, displayRules.Other.Required, displayRules.Other.MinLength),
                    new FieldValidationSetting(!displayRules.Ordered.Show, displayRules.Ordered.Required));
        }
    }
}
