namespace DH.Helpdesk.Services.AggregateDataLoader.Changes
{
    using DH.Helpdesk.Services.AggregateData.Changes;

    public interface IChangeOptionsDataLoader
    {
        ChangeEditOptionsData Load(int customerId, int changeId);
    }
}