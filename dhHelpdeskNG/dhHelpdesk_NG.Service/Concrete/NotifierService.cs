namespace dhHelpdesk_NG.Service.Concrete
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;

    public sealed class NotifierService : INotifierService
    {
        private readonly INotifiersRepository notifiersRepository;

        public NotifierService(INotifiersRepository notifiersRepository)
        {
            this.notifiersRepository = notifiersRepository;
        }

        public void AddNotifier(NewNotifier notifier)
        {
            // Validate dynamic rules

            var newNotifierDto = new NewNotifierDto(
                notifier.CustomerId,
                notifier.UserId,
                notifier.DomainId,
                notifier.LoginName,
                notifier.FirstName,
                notifier.Initials,
                notifier.LastName,
                notifier.DisplayName,
                notifier.Place,
                notifier.Phone,
                notifier.CellPhone,
                notifier.Email,
                notifier.PostalCode,
                notifier.PostalAddress,
                notifier.PostalCode,
                notifier.City,
                notifier.Title,
                notifier.DepartmentId,
                notifier.Unit,
                notifier.OrganizationUnitId,
                notifier.DivisionId,
                notifier.ManagerId,
                notifier.GroupId,
                notifier.Password,
                notifier.Other,
                notifier.Ordered,
                notifier.IsActive,
                notifier.CreatedDate);

            this.notifiersRepository.AddNotifier(newNotifierDto);
            this.notifiersRepository.Commit();
        }

        public void UpdateNotifier(UpdatedNotifier notifier)
        {
            // Validate dynamic rules

            var updatedNotifierDto = new UpdatedNotifierDto(
                notifier.Id,
                notifier.UserId,
                notifier.DomainId,
                notifier.LoginName,
                notifier.FirstName,
                notifier.Initials,
                notifier.LastName,
                notifier.DisplayName,
                notifier.Place,
                notifier.Phone,
                notifier.CellPhone,
                notifier.Email,
                notifier.Code,
                notifier.PostalAddress,
                notifier.PostalCode,
                notifier.City,
                notifier.Title,
                notifier.DepartmentId,
                notifier.Unit,
                notifier.OrganizationUnitId,
                notifier.DivisionId,
                notifier.ManagerId,
                notifier.GroupId,
                notifier.Password,
                notifier.Other,
                notifier.Ordered,
                notifier.IsActive,
                notifier.ChangedDate);

            this.notifiersRepository.UpdateNotifier(updatedNotifierDto);
            this.notifiersRepository.Commit();
        }
    }
}
