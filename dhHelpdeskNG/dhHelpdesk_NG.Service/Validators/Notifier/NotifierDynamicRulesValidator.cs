namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Enums.Notifiers;
    using dhHelpdesk_NG.Data.Exceptions;
    using dhHelpdesk_NG.Service.Validators.Notifier.Settings;

    public sealed class NotifierDynamicRulesValidator : DynamicRulesValidator
    {
        #region Public Methods and Operators

        public void Validate(
            Notifier validatableNotifier, Notifier existingNotifier, FieldValidationSettings validationSettings)
        {
            var errors = new List<FieldValidationError>();

            this.ValidateIntegerField(
                validatableNotifier.DomainId, 
                existingNotifier.DomainId, 
                NotifierField.Domain, 
                validationSettings.Domain, 
                errors);

            this.ValidateStringField(
                validatableNotifier.LoginName, 
                existingNotifier.LoginName, 
                NotifierField.LoginName, 
                validationSettings.LoginName, 
                errors);

            this.ValidateStringField(
                validatableNotifier.FirstName, 
                existingNotifier.FirstName, 
                NotifierField.FirstName, 
                validationSettings.FirstName, 
                errors);

            this.ValidateStringField(
                validatableNotifier.Initials, 
                existingNotifier.Initials, 
                NotifierField.Initials, 
                validationSettings.Initials, 
                errors);

            this.ValidateStringField(
                validatableNotifier.LastName, 
                existingNotifier.LastName, 
                NotifierField.LastName, 
                validationSettings.LastName, 
                errors);

            this.ValidateStringField(
                validatableNotifier.DisplayName, 
                existingNotifier.DisplayName, 
                NotifierField.DisplayName, 
                validationSettings.DisplayName, 
                errors);

            this.ValidateStringField(
                validatableNotifier.Place, existingNotifier.Place, NotifierField.Place, validationSettings.Place, errors);

            this.ValidateStringField(
                validatableNotifier.Phone, existingNotifier.Phone, NotifierField.Phone, validationSettings.Phone, errors);

            this.ValidateStringField(
                validatableNotifier.CellPhone, 
                existingNotifier.CellPhone, 
                NotifierField.CellPhone, 
                validationSettings.CellPhone, 
                errors);

            this.ValidateStringField(
                validatableNotifier.Email, existingNotifier.Email, NotifierField.Email, validationSettings.Email, errors);

            this.ValidateStringField(
                validatableNotifier.Code, existingNotifier.Code, NotifierField.Code, validationSettings.Code, errors);

            this.ValidateStringField(
                validatableNotifier.PostalAddress, 
                existingNotifier.PostalAddress, 
                NotifierField.Address, 
                validationSettings.PostalAddress, 
                errors);

            this.ValidateStringField(
                validatableNotifier.PostalCode, 
                existingNotifier.PostalCode, 
                NotifierField.PostalCode, 
                validationSettings.PostalCode, 
                errors);

            this.ValidateStringField(
                validatableNotifier.City, existingNotifier.City, NotifierField.City, validationSettings.City, errors);

            this.ValidateStringField(
                validatableNotifier.Title, existingNotifier.Title, NotifierField.Title, validationSettings.Title, errors);

            this.ValidateIntegerField(
                validatableNotifier.DepartmentId, 
                existingNotifier.DepartmentId, 
                NotifierField.Department, 
                validationSettings.Department, 
                errors);

            this.ValidateStringField(
                validatableNotifier.Unit, existingNotifier.Unit, NotifierField.Unit, validationSettings.Unit, errors);

            this.ValidateIntegerField(
                validatableNotifier.OrganizationUnitId, 
                existingNotifier.OrganizationUnitId, 
                NotifierField.OrganizationUnit, 
                validationSettings.OrganizationUnit, 
                errors);

            this.ValidateIntegerField(
                validatableNotifier.DivisionId, 
                existingNotifier.DivisionId, 
                NotifierField.Division, 
                validationSettings.Division, 
                errors);

            this.ValidateIntegerField(
                validatableNotifier.ManagerId, 
                existingNotifier.ManagerId, 
                NotifierField.Manager, 
                validationSettings.Manager, 
                errors);

            this.ValidateIntegerField(
                validatableNotifier.GroupId, 
                existingNotifier.GroupId, 
                NotifierField.Group, 
                validationSettings.Group, 
                errors);

            this.ValidateStringField(
                validatableNotifier.Password, 
                existingNotifier.Password, 
                NotifierField.Password, 
                validationSettings.Password, 
                errors);

            this.ValidateStringField(
                validatableNotifier.Other, existingNotifier.Other, NotifierField.Other, validationSettings.Other, errors);

            this.ValidateBoolean(
                validatableNotifier.Ordered,
                existingNotifier.Ordered,
                NotifierField.Ordered,
                validationSettings.Ordered,
                errors);

            if (errors.Any())
            {
                throw new EntityDynamicValidationRulesException(errors, "Failed dynamic rules entity validation.");
            }
        }

        #endregion
    }
}