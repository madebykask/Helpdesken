namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface IUpdatedChangeAggregateFactory
    {
        UpdatedChangeAggregate Create(ChangeModel model, DateTime changedDate);
    }
}