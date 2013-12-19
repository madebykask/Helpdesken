namespace dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public interface ISettingsInputModelToUpdatedFieldSettingsConverter
    {
        UpdatedFieldSettingsDto Convert(SettingsInputModel model, DateTime changedDateTime, int customerId);
    }
}