namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;

    public interface IChangeAggregateFactory
    {
        ChangeAggregate Create(Change change, List<Contact> contacts, List<HistoriesDifference> histories);
    }
}