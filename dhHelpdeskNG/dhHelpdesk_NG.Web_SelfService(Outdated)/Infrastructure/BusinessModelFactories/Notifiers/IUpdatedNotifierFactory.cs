namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.SelfService.Models.Notifiers;

    public interface IUpdatedNotifierFactory
    {
        Notifier Create(InputModel model, DateTime changedDateAndTime);
    }
}