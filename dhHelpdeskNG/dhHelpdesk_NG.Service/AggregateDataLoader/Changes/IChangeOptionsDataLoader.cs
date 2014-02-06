namespace dhHelpdesk_NG.Service.AggregateDataLoader.Changes
{
    using dhHelpdesk_NG.Service.AggregateData.Changes;

    public interface IChangeOptionsDataLoader
    {
        ChangeEditOptionsData Load(int customerId, int changeId);
    }
}