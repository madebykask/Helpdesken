namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public interface IUpdatedFieldSettingsFactory
    {
        UpdatedFieldSettingsDto Convert(SettingsInputModel model, DateTime changedDateTime, int customerId);
    }
}