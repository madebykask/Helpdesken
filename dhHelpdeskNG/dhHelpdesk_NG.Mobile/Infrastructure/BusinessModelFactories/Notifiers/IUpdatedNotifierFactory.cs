namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Mobile.Models.Notifiers;

    public interface IUpdatedNotifierFactory
    {
        Notifier Create(InputModel model, DateTime changedDateAndTime);
    }
}