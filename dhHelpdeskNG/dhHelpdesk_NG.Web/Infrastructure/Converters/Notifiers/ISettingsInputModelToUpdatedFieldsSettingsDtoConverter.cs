namespace dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public interface ISettingsInputModelToUpdatedFieldsSettingsDtoConverter
    {
        UpdatedFieldsSettingsDto Convert(SettingsInputModel model, DateTime changedDate, int customerId);
    }
}