namespace DH.Helpdesk.Services.Validators.Notifier.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;
    using DH.Helpdesk.Dal.Enums.Notifiers;
    using DH.Helpdesk.Services.Validators.Common;

    using DH.Helpdesk.BusinessData.Models.Notifiers;

    public sealed class NotifierElementaryRulesValidator : INotifierDynamicRulesValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public NotifierElementaryRulesValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        #region Public Methods and Operators

        public void Validate(Notifier updatedNotifier, Notifier existingNotifier, NotifierProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DomainId,
                existingNotifier.DomainId,
                NotifierField.Domain,
                CreateValidationRule(settings.Domain));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.LoginName,
                existingNotifier.LoginName,
                NotifierField.LoginName,
                CreateValidationRule(settings.LoginName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.FirstName,
                existingNotifier.FirstName,
                NotifierField.FirstName,
                CreateValidationRule(settings.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Initials,
                existingNotifier.Initials,
                NotifierField.Initials,
                CreateValidationRule(settings.Initials));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.LastName,
                existingNotifier.LastName,
                NotifierField.LastName,
                CreateValidationRule(settings.LastName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.DisplayName,
                existingNotifier.DisplayName,
                NotifierField.DisplayName,
                CreateValidationRule(settings.DisplayName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Place,
                existingNotifier.Place,
                NotifierField.Place,
                CreateValidationRule(settings.Place));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Phone,
                existingNotifier.Phone,
                NotifierField.Phone,
                CreateValidationRule(settings.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.CellPhone,
                existingNotifier.CellPhone,
                NotifierField.CellPhone,
                CreateValidationRule(settings.CellPhone));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Email,
                existingNotifier.Email,
                NotifierField.Email,
                CreateValidationRule(settings.Email));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Code,
                existingNotifier.Code,
                NotifierField.Code,
                CreateValidationRule(settings.Code));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.PostalAddress,
                existingNotifier.PostalAddress,
                NotifierField.PostalAddress,
                CreateValidationRule(settings.PostalAddress));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.PostalCode,
                existingNotifier.PostalCode,
                NotifierField.PostalCode,
                CreateValidationRule(settings.PostalCode));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.City,
                existingNotifier.City,
                NotifierField.City,
                CreateValidationRule(settings.City));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Title,
                existingNotifier.Title,
                NotifierField.Title,
                CreateValidationRule(settings.Title));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DepartmentId,
                existingNotifier.DepartmentId,
                NotifierField.Department,
                CreateValidationRule(settings.Department));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Unit,
                existingNotifier.Unit,
                NotifierField.Unit,
                CreateValidationRule(settings.Unit));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.OrganizationUnitId,
                existingNotifier.OrganizationUnitId,
                NotifierField.OrganizationUnit,
                CreateValidationRule(settings.OrganizationUnit));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DivisionId,
                existingNotifier.DivisionId,
                NotifierField.Division,
                CreateValidationRule(settings.Division));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.ManagerId,
                existingNotifier.ManagerId,
                NotifierField.Manager,
                CreateValidationRule(settings.Manager));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.GroupId,
                existingNotifier.GroupId,
                NotifierField.Group,
                CreateValidationRule(settings.Group));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Other,
                existingNotifier.Other,
                NotifierField.Other,
                CreateValidationRule(settings.Other));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedNotifier.Ordered,
                existingNotifier.Ordered,
                NotifierField.Ordered,
                CreateValidationRule(settings.Ordered));
        }

        public void Validate(Notifier validatableNotifier, NotifierProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DomainId,
                NotifierField.Domain,
                CreateValidationRule(settings.Domain));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.LoginName,
                NotifierField.LoginName,
                CreateValidationRule(settings.LoginName));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.FirstName,
                NotifierField.FirstName,
                CreateValidationRule(settings.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Initials,
                NotifierField.Initials,
                CreateValidationRule(settings.Initials));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.LastName,
                NotifierField.LastName,
                CreateValidationRule(settings.LastName));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.DisplayName,
                NotifierField.DisplayName,
                CreateValidationRule(settings.DisplayName));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Place,
                NotifierField.Place,
                CreateValidationRule(settings.Place));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Phone,
                NotifierField.Phone,
                CreateValidationRule(settings.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.CellPhone,
                NotifierField.CellPhone,
                CreateValidationRule(settings.CellPhone));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Email,
                NotifierField.Email,
                CreateValidationRule(settings.Email));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Code,
                NotifierField.Code,
                CreateValidationRule(settings.Code));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.PostalAddress,
                NotifierField.PostalAddress,
                CreateValidationRule(settings.PostalAddress));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.PostalCode,
                NotifierField.PostalCode,
                CreateValidationRule(settings.PostalCode));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.City,
                NotifierField.City,
                CreateValidationRule(settings.City));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Title,
                NotifierField.Title,
                CreateValidationRule(settings.Title));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DepartmentId,
                NotifierField.Department,
                CreateValidationRule(settings.Department));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Unit,
                NotifierField.Unit,
                CreateValidationRule(settings.Unit));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.OrganizationUnitId,
                NotifierField.OrganizationUnit,
                CreateValidationRule(settings.OrganizationUnit));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DivisionId,
                NotifierField.Division,
                CreateValidationRule(settings.Division));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.ManagerId,
                NotifierField.Manager,
                CreateValidationRule(settings.Manager));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.GroupId,
                NotifierField.Group,
                CreateValidationRule(settings.Group));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Other,
                NotifierField.Other,
                CreateValidationRule(settings.Other));
        }

        private static ElementaryValidationRule CreateValidationRule(FieldProcessingSetting setting)
        {
            return new ElementaryValidationRule(!setting.Show, setting.Required);
        }

        #endregion
    }
}