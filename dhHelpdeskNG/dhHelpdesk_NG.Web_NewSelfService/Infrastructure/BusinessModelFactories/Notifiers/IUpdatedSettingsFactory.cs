namespace DH.Helpdesk.NewSelfService.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.NewSelfService.Models.Notifiers;

    public interface IUpdatedSettingsFactory
    {
        FieldSettings Create(SettingsModel model, int customerId, DateTime changedDateAndTime);
    }
}