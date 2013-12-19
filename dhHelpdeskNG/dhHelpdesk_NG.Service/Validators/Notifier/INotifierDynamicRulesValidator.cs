namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(
            UpdatedNotifierDto updatedNotifier, ExistingNotifierDto existingNotifier, FieldValidationSettings validationSettings);

        void Validate(NewNotifierDto newNotifier, FieldValidationSettings validationSettings);
    }
}