namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Mobile.Models.Notifiers;

    public interface IUpdatedSettingsFactory
    {
        FieldSettings Create(SettingsModel model, int customerId, DateTime changedDateAndTime);
    }
}