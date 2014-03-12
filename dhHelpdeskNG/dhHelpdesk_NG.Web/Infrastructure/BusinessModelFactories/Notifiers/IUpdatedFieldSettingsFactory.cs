namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.Web.Models.Notifiers.Input;

    public interface IUpdatedFieldSettingsFactory
    {
        UpdatedFieldSettings Convert(SettingsInputModel model, DateTime changedDateAndTime, int customerId);
    }
}