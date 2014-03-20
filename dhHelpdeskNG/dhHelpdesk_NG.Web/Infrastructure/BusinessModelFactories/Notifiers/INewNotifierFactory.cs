namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface INewNotifierFactory
    {
        Notifier Create(InputModel model, int customerId, DateTime createdDateAndTime);
    }
}