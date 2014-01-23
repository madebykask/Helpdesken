namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChangeAggregate;

    public interface IUpdatedChangeFactory
    {
        UpdatedChange Create(UpdatedChangeAggregate updatedChange);
    }
}
