namespace DH.Helpdesk.Services.AggregateDataLoader.Changes
{
    using DH.Helpdesk.Services.AggregateData.Changes;

    public interface IChangeAggregateDataLoader
    {
        ChangeAggregateData Load(int changeId);
    }
}