namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface IUpdatedNotifierFactory
    {
        Notifier Create(InputModel model, DateTime changedDateAndTime);
    }
}