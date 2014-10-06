namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.SettingsEdit;

    public interface IUpdatedSettingsFactory
    {
        #region Public Methods and Operators

        ChangeFieldSettings Create(SettingsModel model, OperationContext context);

        #endregion
    }
}