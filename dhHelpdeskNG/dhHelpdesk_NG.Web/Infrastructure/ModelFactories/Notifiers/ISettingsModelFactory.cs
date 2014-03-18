namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface ISettingsModelFactory
    {
        #region Public Methods and Operators

        SettingsModel Create(FieldSettings settings, List<ItemOverview> languages, int selectedLanguageId);

        #endregion
    }
}