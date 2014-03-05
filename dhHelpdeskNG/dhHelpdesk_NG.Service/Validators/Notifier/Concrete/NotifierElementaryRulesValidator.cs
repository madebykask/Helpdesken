namespace DH.Helpdesk.Services.Validators.Notifier.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Enums.Notifiers;
    using DH.Helpdesk.Services.Validators.Common;

    public sealed class NotifierElementaryRulesValidator : INotifierDynamicRulesValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public NotifierElementaryRulesValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        #region Public Methods and Operators

        public void Validate(
            UpdatedNotifierDto updatedNotifier,
            ExistingNotifierDto existingNotifier,
            FieldValidationSettings validationSettings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DomainId, existingNotifier.DomainId, NotifierField.Domain, validationSettings.Domain);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.LoginName,
                existingNotifier.LoginName,
                NotifierField.LoginName,
                validationSettings.LoginName);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.FirstName,
                existingNotifier.FirstName,
                NotifierField.FirstName,
                validationSettings.FirstName);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Initials, existingNotifier.Initials, NotifierField.Initials, validationSettings.Initials);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.LastName, existingNotifier.LastName, NotifierField.LastName, validationSettings.LastName);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.DisplayName,
                existingNotifier.DisplayName,
                NotifierField.DisplayName,
                validationSettings.DisplayName);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Place, existingNotifier.Place, NotifierField.Place, validationSettings.Place);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Phone, existingNotifier.Phone, NotifierField.Phone, validationSettings.Phone);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.CellPhone,
                existingNotifier.CellPhone,
                NotifierField.CellPhone,
                validationSettings.CellPhone);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Email, existingNotifier.Email, NotifierField.Email, validationSettings.Email);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Code, existingNotifier.Code, NotifierField.Code, validationSettings.Code);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.PostalAddress,
                existingNotifier.PostalAddress,
                NotifierField.PostalAddress,
                validationSettings.PostalAddress);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.PostalCode,
                existingNotifier.PostalCode,
                NotifierField.PostalCode,
                validationSettings.PostalCode);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.City, existingNotifier.City, NotifierField.City, validationSettings.City);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Title, existingNotifier.Title, NotifierField.Title, validationSettings.Title);

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DepartmentId,
                existingNotifier.DepartmentId,
                NotifierField.Department,
                validationSettings.Department);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Unit, existingNotifier.Unit, NotifierField.Unit, validationSettings.Unit);

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.OrganizationUnitId,
                existingNotifier.OrganizationUnitId,
                NotifierField.OrganizationUnit,
                validationSettings.OrganizationUnit);

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DivisionId,
                existingNotifier.DivisionId,
                NotifierField.Division,
                validationSettings.Division);

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.ManagerId, existingNotifier.ManagerId, NotifierField.Manager, validationSettings.Manager);

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.GroupId, existingNotifier.GroupId, NotifierField.Group, validationSettings.Group);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Password, existingNotifier.Password, NotifierField.Password, validationSettings.Password);

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Other, existingNotifier.Other, NotifierField.Other, validationSettings.Other);

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedNotifier.Ordered, existingNotifier.Ordered, NotifierField.Ordered, validationSettings.Ordered);
        }

        public void Validate(NewNotifierDto validatableNotifier, FieldValidationSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DomainId, NotifierField.Domain, settings.Domain);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.LoginName, NotifierField.LoginName, settings.LoginName);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.FirstName, NotifierField.FirstName, settings.FirstName);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Initials, NotifierField.Initials, settings.Initials);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.LastName, NotifierField.LastName, settings.LastName);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.DisplayName, NotifierField.DisplayName, settings.DisplayName);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Place, NotifierField.Place, settings.Place);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Phone, NotifierField.Phone, settings.Phone);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.CellPhone, NotifierField.CellPhone, settings.CellPhone);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Email, NotifierField.Email, settings.Email);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Code, NotifierField.Code, settings.Code);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.PostalAddress, NotifierField.PostalAddress, settings.PostalAddress);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.PostalCode, NotifierField.PostalCode, settings.PostalCode);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.City, NotifierField.City, settings.City);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Title, NotifierField.Title, settings.Title);

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DepartmentId, NotifierField.Department, settings.Department);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Unit, NotifierField.Unit, settings.Unit);

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.OrganizationUnitId, NotifierField.OrganizationUnit, settings.OrganizationUnit);

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DivisionId, NotifierField.Division, settings.Division);

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.ManagerId, NotifierField.Manager, settings.Manager);

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.GroupId, NotifierField.Group, settings.Group);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Password, NotifierField.Password, settings.Password);

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Other, NotifierField.Other, settings.Other);
        }

        #endregion
    }
}