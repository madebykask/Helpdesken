namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public interface IUpdatedSettingsFactory
    {
        UpdatedSettings Create(
            SettingsModel model, int currentCustomerId, int currentLanguageId, DateTime changedDateAndTime);
    }
}