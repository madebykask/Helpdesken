namespace dhHelpdesk_NG.Service.AggregateDataLoader.Changes
{
    using dhHelpdesk_NG.Service.AggregateData.Changes;

    public interface IChangeAggregateDataLoader
    {
        ChangeAggregateData Load(int changeId);
    }
}