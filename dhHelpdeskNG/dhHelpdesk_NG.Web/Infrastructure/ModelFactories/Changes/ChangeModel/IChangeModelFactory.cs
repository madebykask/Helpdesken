namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface IChangeModelFactory
    {
        ChangeModel Create(ChangeAggregate change, ChangeEditOptions optionalData, ChangeEditSettings editSettings);
    }
}