namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface INewChangeAggregateFactory
    {
        NewChangeAggregate Create(NewChangeModel model, DateTime createdDate);
    }
}
