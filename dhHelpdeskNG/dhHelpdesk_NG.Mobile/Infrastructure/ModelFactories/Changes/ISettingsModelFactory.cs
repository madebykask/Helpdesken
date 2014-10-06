namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Mobile.Models.Changes.SettingsEdit;

    public interface ISettingsModelFactory
    {
        #region Public Methods and Operators

        SettingsModel Create(GetSettingsResponse response);

        #endregion
    }
}