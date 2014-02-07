namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;

    public interface INotifierService
    {
        void AddNotifier(NewNotifierDto notifier);

        void UpdateNotifier(UpdatedNotifierDto notifier, int customerId);
    }
}
