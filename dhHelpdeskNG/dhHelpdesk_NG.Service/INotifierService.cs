namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;

    public interface INotifierService
    {
        void AddNotifier(NewNotifier notifier);

        void UpdateNotifier(UpdatedNotifier notifier);
    }
}
