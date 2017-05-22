namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;

    public sealed class NotifierRestorer : Restorer, IRestorer<Notifier, NotifierProcessingSettings>
    {
        public void Restore(Notifier updatedNotifier, Notifier existingNotifier, NotifierProcessingSettings settings)
        {
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.UserId, existingNotifier.UserId, settings.UserId.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.DomainId, existingNotifier.DomainId, settings.Domain.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.LoginName, existingNotifier.LoginName, settings.LoginName.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.FirstName, existingNotifier.FirstName, settings.FirstName.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Initials, existingNotifier.Initials, settings.Initials.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.LastName, existingNotifier.LastName, settings.LastName.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.DisplayName, existingNotifier.DisplayName, settings.DisplayName.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Place, existingNotifier.Place, settings.Place.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Phone, existingNotifier.Phone, settings.Phone.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.CellPhone, existingNotifier.CellPhone, settings.CellPhone.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Email, existingNotifier.Email, settings.Email.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Code, existingNotifier.Code, settings.Code.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.PostalAddress, existingNotifier.PostalAddress, settings.PostalAddress.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.PostalCode, existingNotifier.PostalCode, settings.PostalCode.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.City, existingNotifier.City, settings.City.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Title, existingNotifier.Title, settings.Title.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.DepartmentId, existingNotifier.DepartmentId, settings.Department.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Unit, existingNotifier.Unit, settings.Unit.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.OrganizationUnitId, existingNotifier.OrganizationUnitId, settings.OrganizationUnit.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.DivisionId, existingNotifier.DivisionId, settings.Division.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.ManagerId, existingNotifier.ManagerId, settings.Manager.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.GroupId, existingNotifier.GroupId, settings.Group.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Other, existingNotifier.Other, settings.Other.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.Ordered, existingNotifier.Ordered, settings.Ordered.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.ChangedDateAndTime, existingNotifier.ChangedDateAndTime, settings.ChangedDate.Show);
            this.RestoreFieldIfNeeded(updatedNotifier, () => updatedNotifier.LanguageId, existingNotifier.LanguageId, settings.Language.Show);
        }
    }
}