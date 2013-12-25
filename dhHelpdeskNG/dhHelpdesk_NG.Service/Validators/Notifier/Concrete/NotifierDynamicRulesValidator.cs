namespace dhHelpdesk_NG.Service.Validators.Notifier.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Common.Exceptions;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Enums.Notifiers;
    using ExistingNotifierDto = dhHelpdesk_NG.DTO.DTOs.Notifiers.Output.ExistingNotifierDto;

    public sealed class NotifierDynamicRulesValidator : DynamicRulesValidator, INotifierDynamicRulesValidator
    {
        #region Public Methods and Operators

        public void Validate(
            UpdatedNotifierDto updatedNotifier, ExistingNotifierDto existingNotifier, FieldValidationSettings validationSettings)
        {
            var errors = new List<FieldValidationError>();

            this.ValidateIntegerField(
                updatedNotifier.DomainId, 
                existingNotifier.DomainId, 
                NotifierField.Domain, 
                validationSettings.Domain, 
                errors);

            this.ValidateStringField(
                updatedNotifier.LoginName, 
                existingNotifier.LoginName, 
                NotifierField.LoginName, 
                validationSettings.LoginName, 
                errors);

            this.ValidateStringField(
                updatedNotifier.FirstName, 
                existingNotifier.FirstName, 
                NotifierField.FirstName, 
                validationSettings.FirstName, 
                errors);

            this.ValidateStringField(
                updatedNotifier.Initials, 
                existingNotifier.Initials, 
                NotifierField.Initials, 
                validationSettings.Initials, 
                errors);

            this.ValidateStringField(
                updatedNotifier.LastName, 
                existingNotifier.LastName, 
                NotifierField.LastName, 
                validationSettings.LastName, 
                errors);

            this.ValidateStringField(
                updatedNotifier.DisplayName, 
                existingNotifier.DisplayName, 
                NotifierField.DisplayName, 
                validationSettings.DisplayName, 
                errors);

            this.ValidateStringField(
                updatedNotifier.Place, existingNotifier.Place, NotifierField.Place, validationSettings.Place, errors);

            this.ValidateStringField(
                updatedNotifier.Phone, existingNotifier.Phone, NotifierField.Phone, validationSettings.Phone, errors);

            this.ValidateStringField(
                updatedNotifier.CellPhone, 
                existingNotifier.CellPhone, 
                NotifierField.CellPhone, 
                validationSettings.CellPhone, 
                errors);

            this.ValidateStringField(
                updatedNotifier.Email, existingNotifier.Email, NotifierField.Email, validationSettings.Email, errors);

            this.ValidateStringField(
                updatedNotifier.Code, existingNotifier.Code, NotifierField.Code, validationSettings.Code, errors);

            this.ValidateStringField(
                updatedNotifier.PostalAddress, 
                existingNotifier.PostalAddress, 
                NotifierField.PostalAddress, 
                validationSettings.PostalAddress, 
                errors);

            this.ValidateStringField(
                updatedNotifier.PostalCode, 
                existingNotifier.PostalCode, 
                NotifierField.PostalCode, 
                validationSettings.PostalCode, 
                errors);

            this.ValidateStringField(
                updatedNotifier.City, existingNotifier.City, NotifierField.City, validationSettings.City, errors);

            this.ValidateStringField(
                updatedNotifier.Title, existingNotifier.Title, NotifierField.Title, validationSettings.Title, errors);

            this.ValidateIntegerField(
                updatedNotifier.DepartmentId, 
                existingNotifier.DepartmentId, 
                NotifierField.Department, 
                validationSettings.Department, 
                errors);

            this.ValidateStringField(
                updatedNotifier.Unit, existingNotifier.Unit, NotifierField.Unit, validationSettings.Unit, errors);

            this.ValidateIntegerField(
                updatedNotifier.OrganizationUnitId, 
                existingNotifier.OrganizationUnitId, 
                NotifierField.OrganizationUnit, 
                validationSettings.OrganizationUnit, 
                errors);

            this.ValidateIntegerField(
                updatedNotifier.DivisionId, 
                existingNotifier.DivisionId, 
                NotifierField.Division, 
                validationSettings.Division, 
                errors);

            this.ValidateIntegerField(
                updatedNotifier.ManagerId, 
                existingNotifier.ManagerId, 
                NotifierField.Manager, 
                validationSettings.Manager, 
                errors);

            this.ValidateIntegerField(
                updatedNotifier.GroupId, 
                existingNotifier.GroupId, 
                NotifierField.Group, 
                validationSettings.Group, 
                errors);

            this.ValidateStringField(
                updatedNotifier.Password, 
                existingNotifier.Password, 
                NotifierField.Password, 
                validationSettings.Password, 
                errors);

            this.ValidateStringField(
                updatedNotifier.Other, existingNotifier.Other, NotifierField.Other, validationSettings.Other, errors);

            this.ValidateBooleanField(
                updatedNotifier.Ordered,
                existingNotifier.Ordered,
                NotifierField.Ordered,
                validationSettings.Ordered,
                errors);

            if (errors.Any())
            {
                throw new EntityDynamicValidationRulesException(errors, "Failed dynamic rules entity validation.");
            }
        }

        public void Validate(NewNotifierDto validatableNotifier, FieldValidationSettings settings)
        {
            var errors = new List<FieldValidationError>();

            this.ValidateIntegerField(
                validatableNotifier.DomainId,
                NotifierField.Domain,
                settings.Domain,
                errors);

            this.ValidateStringField(
                validatableNotifier.LoginName,
                NotifierField.LoginName,
                settings.LoginName,
                errors);

            this.ValidateStringField(
                validatableNotifier.FirstName,
                NotifierField.FirstName,
                settings.FirstName,
                errors);

            this.ValidateStringField(
                validatableNotifier.Initials,
                NotifierField.Initials,
                settings.Initials,
                errors);

            this.ValidateStringField(
                validatableNotifier.LastName,
                NotifierField.LastName,
                settings.LastName,
                errors);

            this.ValidateStringField(
                validatableNotifier.DisplayName,
                NotifierField.DisplayName,
                settings.DisplayName,
                errors);

            this.ValidateStringField(
                validatableNotifier.Place, NotifierField.Place, settings.Place, errors);

            this.ValidateStringField(
                validatableNotifier.Phone, NotifierField.Phone, settings.Phone, errors);

            this.ValidateStringField(
                validatableNotifier.CellPhone,
                NotifierField.CellPhone,
                settings.CellPhone,
                errors);

            this.ValidateStringField(
                validatableNotifier.Email, NotifierField.Email, settings.Email, errors);

            this.ValidateStringField(
                validatableNotifier.Code, NotifierField.Code, settings.Code, errors);

            this.ValidateStringField(
                validatableNotifier.PostalAddress,
                NotifierField.PostalAddress,
                settings.PostalAddress,
                errors);

            this.ValidateStringField(
                validatableNotifier.PostalCode,
                NotifierField.PostalCode,
                settings.PostalCode,
                errors);

            this.ValidateStringField(
                validatableNotifier.City, NotifierField.City, settings.City, errors);

            this.ValidateStringField(
                validatableNotifier.Title, NotifierField.Title, settings.Title, errors);

            this.ValidateIntegerField(
                validatableNotifier.DepartmentId,
                NotifierField.Department,
                settings.Department,
                errors);

            this.ValidateStringField(
                validatableNotifier.Unit, NotifierField.Unit, settings.Unit, errors);

            this.ValidateIntegerField(
                validatableNotifier.OrganizationUnitId,
                NotifierField.OrganizationUnit,
                settings.OrganizationUnit,
                errors);

            this.ValidateIntegerField(
                validatableNotifier.DivisionId,
                NotifierField.Division,
                settings.Division,
                errors);

            this.ValidateIntegerField(
                validatableNotifier.ManagerId,
                NotifierField.Manager,
                settings.Manager,
                errors);

            this.ValidateIntegerField(
                validatableNotifier.GroupId,
                NotifierField.Group,
                settings.Group,
                errors);

            this.ValidateStringField(
                validatableNotifier.Password,
                NotifierField.Password,
                settings.Password,
                errors);

            this.ValidateStringField(
                validatableNotifier.Other, NotifierField.Other, settings.Other, errors);

            if (errors.Any())
            {
                throw new EntityDynamicValidationRulesException(errors, "Failed dynamic rules entity validation.");
            }
        }

        #endregion
    }
}