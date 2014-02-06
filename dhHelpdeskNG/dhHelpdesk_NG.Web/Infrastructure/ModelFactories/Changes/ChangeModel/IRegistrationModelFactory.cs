namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public interface IRegistrationModelFactory
    {
        RegistrationModel Create(
            string temporaryId,
            RegistrationFieldEditSettings editSettings,
            ChangeOptionalData optionalData);

        RegistrationModel Create(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings,
            ChangeOptionalData optionalData);
    }
}