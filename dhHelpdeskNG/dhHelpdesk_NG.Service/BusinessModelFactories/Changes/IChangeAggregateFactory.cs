namespace DH.Helpdesk.Services.BusinessModelFactories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;

    public interface IChangeAggregateFactory
    {
        ChangeAggregate Create(Change change, List<Contact> contacts, List<HistoriesDifference> histories);
    }
}