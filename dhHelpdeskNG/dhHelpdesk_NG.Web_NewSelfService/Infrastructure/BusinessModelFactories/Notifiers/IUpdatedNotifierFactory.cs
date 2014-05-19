namespace DH.Helpdesk.NewSelfService.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.NewSelfService.Models.Notifiers;

    public interface IUpdatedNotifierFactory
    {
        Notifier Create(InputModel model, DateTime changedDateAndTime);
    }
}