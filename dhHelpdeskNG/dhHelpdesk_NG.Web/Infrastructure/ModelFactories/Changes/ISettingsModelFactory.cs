namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public interface ISettingsModelFactory
    {
        SettingsModel Create(FieldSettings fieldSettings);
    }
}