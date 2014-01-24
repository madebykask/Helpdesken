namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;

    public interface INewChangeFactory
    {
        NewChange Create(NewChangeAggregate newChange);
    }
}
