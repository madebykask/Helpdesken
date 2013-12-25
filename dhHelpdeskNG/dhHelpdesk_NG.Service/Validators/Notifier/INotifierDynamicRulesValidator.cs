namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;

    using ExistingNotifierDto = dhHelpdesk_NG.DTO.DTOs.Notifiers.Output.ExistingNotifierDto;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(
            UpdatedNotifierDto updatedNotifier, ExistingNotifierDto existingNotifier, FieldValidationSettings validationSettings);

        void Validate(NewNotifierDto newNotifier, FieldValidationSettings validationSettings);
    }
}