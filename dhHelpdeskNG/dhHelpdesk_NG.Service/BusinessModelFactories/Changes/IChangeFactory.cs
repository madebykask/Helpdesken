namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IChangeFactory
    {
        ChangeAggregate Create(Change change, List<Contact> contacts);
    }
}