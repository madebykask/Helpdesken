namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate;

    public interface IUpdatedChangeFactory
    {
        UpdatedChange Create(UpdatedChangeAggregate updatedChange);
    }
}
