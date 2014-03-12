namespace DH.Helpdesk.Services.Validators.Notifier
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;

    public interface INotifierDynamicRulesValidator
    {
        void Validate(
            UpdatedNotifier updatedNotifier, ExistingNotifier existingNotifier, FieldValidationSettings validationSettings);

        void Validate(NewNotifier newNotifier, FieldValidationSettings validationSettings);
    }
}