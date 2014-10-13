namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Mobile.Models.Notifiers;

    public interface ISettingsModelFactory
    {
        #region Public Methods and Operators

        SettingsModel Create(FieldSettings settings, List<ItemOverview> languages, int selectedLanguageId);

        #endregion
    }
}