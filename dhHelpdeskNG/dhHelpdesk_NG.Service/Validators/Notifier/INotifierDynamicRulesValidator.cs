namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.Service.Validators.Notifier.Settings;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(
            Notifier validatableNotifier, Notifier existingNotifier, FieldValidationSettings validationSettings);

        void Validate(NewNotifier validatableNotifier, FieldValidationSettings validationSettings);
    }
}