namespace DH.Helpdesk.Services.Validators.Notifier
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(
            UpdatedNotifierDto updatedNotifier, ExistingNotifierDto existingNotifier, FieldValidationSettings validationSettings);

        void Validate(NewNotifierDto newNotifier, FieldValidationSettings validationSettings);
    }
}