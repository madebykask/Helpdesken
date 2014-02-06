namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Settings;

    public interface IUpdatedFieldSettingsFactory
    {
        UpdatedFieldSettingsDto Create(SettingsModel settings, int customerId, int languageId, DateTime changedDateTime);
    }
}