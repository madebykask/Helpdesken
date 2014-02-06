namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public interface IImplementationModelFactory
    {
        ImplementationModel Create(
            string temporaryId,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);

        ImplementationModel Create(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);
    }
}