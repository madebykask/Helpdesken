namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public interface ISettingsModelFactory
    {
        #region Public Methods and Operators

        SettingsModel Create(ChangeFieldSettings settings);

        #endregion
    }
}