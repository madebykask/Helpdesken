namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.Web.Models.Notifiers.Input;

    public interface IUpdatedFieldSettingsFactory
    {
        UpdatedFieldSettingsDto Convert(SettingsInputModel model, DateTime changedDateTime, int customerId);
    }
}