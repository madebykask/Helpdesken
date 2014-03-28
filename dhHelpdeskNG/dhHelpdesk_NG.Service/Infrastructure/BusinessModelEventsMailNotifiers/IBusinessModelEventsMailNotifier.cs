namespace DH.Helpdesk.Services.Infrastructure.BusinessModelEventsMailNotifiers
{
    public interface IBusinessModelEventsMailNotifier<TUpdatedBusinessModel, TExistingBusinessModel>
    {
        void NotifyClients(TUpdatedBusinessModel updated, TExistingBusinessModel existing);
    }
}