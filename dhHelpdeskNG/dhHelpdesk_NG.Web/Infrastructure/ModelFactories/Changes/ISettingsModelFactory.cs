namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Web.Models.Changes.Output;

    public interface ISettingsModelFactory
    {
        SettingsModel Create(FieldSettingsDto fieldSettings);
    }
}