namespace DH.Helpdesk.Services.BusinessModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChangeAggregate;

    public interface INewChangeFactory
    {
        NewChange Create(NewChangeAggregate newChange);
    }
}
