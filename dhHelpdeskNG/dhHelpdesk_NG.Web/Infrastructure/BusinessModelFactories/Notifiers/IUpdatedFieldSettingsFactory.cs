namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Notifiers.Input;

    public interface IUpdatedFieldSettingsFactory
    {
        FieldSettings Convert(SettingsInputModel model, DateTime changedDateAndTime, int customerId);
    }
}