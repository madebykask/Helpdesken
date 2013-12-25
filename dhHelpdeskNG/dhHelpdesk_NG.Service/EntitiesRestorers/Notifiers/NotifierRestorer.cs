namespace dhHelpdesk_NG.Service.EntitiesRestorers.Notifiers
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    public static class NotifierRestorer
    {
        public static void Restore(UpdatedNotifierDto notifier, ExistingNotifierDto existingNotifier, FieldDisplayRulesDto displayRules)
        {
            if (!displayRules.Domain.Show)
            {
                notifier.DomainId = existingNotifier.DomainId;
            }

            if (!displayRules.LoginName.Show)
            {
                notifier.LoginName = existingNotifier.LoginName;
            }

            if (!displayRules.FirstName.Show)
            {
                notifier.FirstName = existingNotifier.FirstName;
            }

            if (!displayRules.Initials.Show)
            {
                notifier.Initials = existingNotifier.Initials;
            }

            if (!displayRules.LastName.Show)
            {
                notifier.LastName = existingNotifier.LastName;
            }

            if (!displayRules.DisplayName.Show)
            {
                notifier.DisplayName = existingNotifier.DisplayName;
            }

            if (!displayRules.Place.Show)
            {
                notifier.Place = existingNotifier.Place;
            }

            if (!displayRules.Phone.Show)
            {
                notifier.Phone = existingNotifier.Phone;
            }

            if (!displayRules.CellPhone.Show)
            {
                notifier.CellPhone = existingNotifier.CellPhone;
            }

            if (!displayRules.Email.Show)
            {
                notifier.Email = existingNotifier.Email;
            }

            if (!displayRules.Code.Show)
            {
                notifier.Code = existingNotifier.Code;
            }

            if (!displayRules.PostalAddress.Show)
            {
                notifier.PostalAddress = existingNotifier.PostalAddress;
            }

            if (!displayRules.PostalCode.Show)
            {
                notifier.PostalCode = existingNotifier.PostalCode;
            }

            if (!displayRules.City.Show)
            {
                notifier.City = existingNotifier.City;
            }

            if (!displayRules.Title.Show)
            {
                notifier.Title = existingNotifier.Title;
            }

            if (!displayRules.Department.Show)
            {
                notifier.DepartmentId = existingNotifier.DepartmentId;
            }

            if (!displayRules.Unit.Show)
            {
                notifier.Unit = existingNotifier.Unit;
            }

            if (!displayRules.OrganizationUnit.Show)
            {
                notifier.OrganizationUnitId = existingNotifier.OrganizationUnitId;
            }

            if (!displayRules.Division.Show)
            {
                notifier.DivisionId = existingNotifier.DivisionId;
            }

            if (!displayRules.Manager.Show)
            {
                notifier.ManagerId = existingNotifier.ManagerId;
            }

            if (!displayRules.Group.Show)
            {
                notifier.GroupId = existingNotifier.GroupId;
            }

            if (!displayRules.Password.Show)
            {
                notifier.Password = existingNotifier.Password;
            }

            if (!displayRules.Other.Show)
            {
                notifier.Other = existingNotifier.Other;
            }

            if (!displayRules.Ordered.Show)
            {
                notifier.Ordered = existingNotifier.Ordered;
            }

            if (!displayRules.ChangedDate.Show)
            {
                notifier.ChangedDate = existingNotifier.ChangedDate;
            }
        }
    }
}
