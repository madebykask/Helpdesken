namespace DH.Helpdesk.Services.AggregateDataLoader.Changes
{
    using DH.Helpdesk.Services.AggregateData.Changes;

    public interface INewChangeOptionsDataLoader
    {
        ChangeEditOptionsData Load(int customerId);
    }
}