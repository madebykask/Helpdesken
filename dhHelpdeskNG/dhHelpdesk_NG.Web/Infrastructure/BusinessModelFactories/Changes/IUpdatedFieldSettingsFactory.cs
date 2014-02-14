namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public interface IUpdatedFieldSettingsFactory
    {
        UpdatedSettings Create(SettingsModel settings, int customerId, int languageId, DateTime changedDateTime);
    }
}