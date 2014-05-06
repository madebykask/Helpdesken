namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes.SettingsEdit;

    public interface ISettingsModelFactory
    {
        #region Public Methods and Operators

        SettingsModel Create(ChangeFieldSettings settings, List<ItemOverview> languages);

        #endregion
    }
}