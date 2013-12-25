namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;

    public interface INotifierService
    {
        void AddNotifier(NewNotifierDto notifier);

        void UpdateNotifier(UpdatedNotifierDto notifier, int customerId);
    }
}
