namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface INewChangeModelFactory
    {
        NewChangeModel Create(string temporatyId, ChangeEditOptions optionalData, ChangeEditSettings editSettings);
    }
}