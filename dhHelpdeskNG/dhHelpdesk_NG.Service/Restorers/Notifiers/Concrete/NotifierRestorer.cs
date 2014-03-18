namespace DH.Helpdesk.Services.Restorers.Notifiers.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;

    public sealed class NotifierRestorer : INotifierRestorer
    {
        public void Restore(Notifier notifier, Notifier existingNotifier, NotifierProcessingSettings settings)
        {
            if (!settings.Domain.Show)
            {
                notifier.DomainId = existingNotifier.DomainId;
            }

            if (!settings.LoginName.Show)
            {
                notifier.LoginName = existingNotifier.LoginName;
            }

            if (!settings.FirstName.Show)
            {
                notifier.FirstName = existingNotifier.FirstName;
            }

            if (!settings.Initials.Show)
            {
                notifier.Initials = existingNotifier.Initials;
            }

            if (!settings.LastName.Show)
            {
                notifier.LastName = existingNotifier.LastName;
            }

            if (!settings.DisplayName.Show)
            {
                notifier.DisplayName = existingNotifier.DisplayName;
            }

            if (!settings.Place.Show)
            {
                notifier.Place = existingNotifier.Place;
            }

            if (!settings.Phone.Show)
            {
                notifier.Phone = existingNotifier.Phone;
            }

            if (!settings.CellPhone.Show)
            {
                notifier.CellPhone = existingNotifier.CellPhone;
            }

            if (!settings.Email.Show)
            {
                notifier.Email = existingNotifier.Email;
            }

            if (!settings.Code.Show)
            {
                notifier.Code = existingNotifier.Code;
            }

            if (!settings.PostalAddress.Show)
            {
                notifier.PostalAddress = existingNotifier.PostalAddress;
            }

            if (!settings.PostalCode.Show)
            {
                notifier.PostalCode = existingNotifier.PostalCode;
            }

            if (!settings.City.Show)
            {
                notifier.City = existingNotifier.City;
            }

            if (!settings.Title.Show)
            {
                notifier.Title = existingNotifier.Title;
            }

            if (!settings.Department.Show)
            {
                notifier.DepartmentId = existingNotifier.DepartmentId;
            }

            if (!settings.Unit.Show)
            {
                notifier.Unit = existingNotifier.Unit;
            }

            if (!settings.OrganizationUnit.Show)
            {
                notifier.OrganizationUnitId = existingNotifier.OrganizationUnitId;
            }

            if (!settings.Division.Show)
            {
                notifier.DivisionId = existingNotifier.DivisionId;
            }

            if (!settings.Manager.Show)
            {
                notifier.ManagerId = existingNotifier.ManagerId;
            }

            if (!settings.Group.Show)
            {
                notifier.GroupId = existingNotifier.GroupId;
            }

            if (!settings.Other.Show)
            {
                notifier.Other = existingNotifier.Other;
            }

            if (!settings.Ordered.Show)
            {
                notifier.Ordered = existingNotifier.Ordered;
            }

            if (!settings.ChangedDate.Show)
            {
                notifier.ChangedDateAndTime = existingNotifier.ChangedDateAndTime;
            }
        }
    }
}
