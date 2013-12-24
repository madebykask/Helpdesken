namespace dhHelpdesk_NG.Service.Converters.Notifiers
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;

    public static class NewNotifierToNewNotifierDtoConverter
    {
        public static NewNotifierDto Convert(NewNotifier notifier, string password)
        {
            return new NewNotifierDto(
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
                password,
                notifier.Other,
                notifier.Ordered,
                notifier.IsActive,
                notifier.CreatedDate);
        }
    }
}
