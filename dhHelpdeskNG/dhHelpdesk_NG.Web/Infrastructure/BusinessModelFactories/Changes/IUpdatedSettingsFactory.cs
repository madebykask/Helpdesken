namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public interface IUpdatedSettingsFactory
    {
        ChangeFieldSettings Create(
            SettingsModel model, int currentCustomerId, int currentLanguageId, DateTime changedDateAndTime);
    }
}