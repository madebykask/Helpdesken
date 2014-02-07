namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChangeAggregate;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Models;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IUpdatedChangeAggregateFactory
    {
        UpdatedChangeAggregate Create(
            ChangeModel model,
            ChangeNewSubitems newSubitems,
            ChangeDeletedSubitems deletedSubitems,
            DateTime changedDate);
    }
}