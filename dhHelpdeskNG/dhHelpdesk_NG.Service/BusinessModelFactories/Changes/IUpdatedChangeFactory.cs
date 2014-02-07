namespace DH.Helpdesk.Services.BusinessModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChangeAggregate;

    public interface IUpdatedChangeFactory
    {
        UpdatedChange Create(UpdatedChangeAggregate updatedChange);
    }
}
