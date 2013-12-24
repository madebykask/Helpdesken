namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(
            UpdatedNotifier updatedNotifier, ExistingNotifierDto existingNotifier, FieldValidationSettings validationSettings);

        void Validate(NewNotifier newNotifier, FieldValidationSettings validationSettings);
    }
}