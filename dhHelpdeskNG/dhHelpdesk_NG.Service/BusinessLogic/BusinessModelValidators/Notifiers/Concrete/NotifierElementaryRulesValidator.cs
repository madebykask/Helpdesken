namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Notifiers.Concrete
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

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
            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.UserId,
                existingNotifier.UserId,
                GeneralField.UserId,
                CreateValidationRule(settings.UserId));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DomainId,
                existingNotifier.DomainId,
                GeneralField.Domain,
                CreateValidationRule(settings.Domain));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.LoginName,
                existingNotifier.LoginName,
                GeneralField.LoginName,
                CreateValidationRule(settings.LoginName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.FirstName,
                existingNotifier.FirstName,
                GeneralField.FirstName,
                CreateValidationRule(settings.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Initials,
                existingNotifier.Initials,
                GeneralField.Initials,
                CreateValidationRule(settings.Initials));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.LastName,
                existingNotifier.LastName,
                GeneralField.LastName,
                CreateValidationRule(settings.LastName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.DisplayName,
                existingNotifier.DisplayName,
                GeneralField.DisplayName,
                CreateValidationRule(settings.DisplayName));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Place,
                existingNotifier.Place,
                GeneralField.Place,
                CreateValidationRule(settings.Place));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Phone,
                existingNotifier.Phone,
                GeneralField.Phone,
                CreateValidationRule(settings.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.CellPhone,
                existingNotifier.CellPhone,
                GeneralField.CellPhone,
                CreateValidationRule(settings.CellPhone));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Email,
                existingNotifier.Email,
                GeneralField.Email,
                CreateValidationRule(settings.Email));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Code,
                existingNotifier.Code,
                GeneralField.Code,
                CreateValidationRule(settings.Code));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.PostalAddress,
                existingNotifier.PostalAddress,
                AddressField.PostalAddress,
                CreateValidationRule(settings.PostalAddress));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.PostalCode,
                existingNotifier.PostalCode,
                AddressField.PostalCode,
                CreateValidationRule(settings.PostalCode));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.City,
                existingNotifier.City,
                AddressField.City,
                CreateValidationRule(settings.City));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Title,
                existingNotifier.Title,
                OrganizationField.Title,
                CreateValidationRule(settings.Title));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DepartmentId,
                existingNotifier.DepartmentId,
                OrganizationField.Department,
                CreateValidationRule(settings.Department));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Unit,
                existingNotifier.Unit,
                OrganizationField.Unit,
                CreateValidationRule(settings.Unit));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.OrganizationUnitId,
                existingNotifier.OrganizationUnitId,
                OrganizationField.OrganizationUnit,
                CreateValidationRule(settings.OrganizationUnit));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.DivisionId,
                existingNotifier.DivisionId,
                OrganizationField.Division,
                CreateValidationRule(settings.Division));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.ManagerId,
                existingNotifier.ManagerId,
                OrganizationField.Manager,
                CreateValidationRule(settings.Manager));

            this.elementaryRulesValidator.ValidateIntegerField(
                updatedNotifier.GroupId,
                existingNotifier.GroupId,
                OrganizationField.Group,
                CreateValidationRule(settings.Group));

            this.elementaryRulesValidator.ValidateStringField(
                updatedNotifier.Other,
                existingNotifier.Other,
                OrganizationField.Other,
                CreateValidationRule(settings.Other));

            this.elementaryRulesValidator.ValidateBooleanField(
                updatedNotifier.Ordered,
                existingNotifier.Ordered,
                OrdererField.Orderer,
                CreateValidationRule(settings.Ordered));


            this.elementaryRulesValidator.ValidateIntegerField(
             updatedNotifier.LanguageId,
             existingNotifier.LanguageId,
             GeneralField.Language,
             CreateValidationRule(settings.Language));
        }

        public void Validate(Notifier validatableNotifier, NotifierProcessingSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.UserId,
                GeneralField.UserId,
                CreateValidationRule(settings.UserId));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DomainId,
                GeneralField.Domain,
                CreateValidationRule(settings.Domain));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.LoginName,
                GeneralField.LoginName,
                CreateValidationRule(settings.LoginName));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.FirstName,
                GeneralField.FirstName,
                CreateValidationRule(new FieldProcessingSetting(true, settings.FirstName.Required)));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Initials,
                GeneralField.Initials,
                CreateValidationRule(settings.Initials));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.LastName,
                GeneralField.LastName,
                CreateValidationRule(new FieldProcessingSetting(true, settings.LastName.Required)));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.DisplayName,
                GeneralField.DisplayName,
                CreateValidationRule(new FieldProcessingSetting(true, settings.DisplayName.Required)));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Place,
                GeneralField.Place,
                CreateValidationRule(settings.Place));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Phone,
                GeneralField.Phone,
                CreateValidationRule(settings.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.CellPhone,
                GeneralField.CellPhone,
                CreateValidationRule(settings.CellPhone));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Email,
                GeneralField.Email,
                CreateValidationRule(settings.Email));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Code,
                GeneralField.Code,
                CreateValidationRule(settings.Code));

            this.elementaryRulesValidator.ValidateStringField(
               validatableNotifier.CostCentre,
               GeneralField.CostCentre,
               CreateValidationRule(settings.CostCentre));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.PostalAddress,
                AddressField.PostalAddress,
                CreateValidationRule(settings.PostalAddress));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.PostalCode,
                AddressField.PostalCode,
                CreateValidationRule(settings.PostalCode));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.City,
                AddressField.City,
                CreateValidationRule(settings.City));            

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Title,
                OrganizationField.Title,
                CreateValidationRule(settings.Title));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DepartmentId,
                OrganizationField.Department,
                CreateValidationRule(settings.Department));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Unit,
                OrganizationField.Unit,
                CreateValidationRule(settings.Unit));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.OrganizationUnitId,
                OrganizationField.OrganizationUnit,
                CreateValidationRule(settings.OrganizationUnit));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.DivisionId,
                OrganizationField.Division,
                CreateValidationRule(settings.Division));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.ManagerId,
                OrganizationField.Manager,
                CreateValidationRule(settings.Manager));

            this.elementaryRulesValidator.ValidateIntegerField(
                validatableNotifier.GroupId,
                OrganizationField.Group,
                CreateValidationRule(settings.Group));

            this.elementaryRulesValidator.ValidateStringField(
                validatableNotifier.Other,
                OrganizationField.Other,
                CreateValidationRule(settings.Other));

            this.elementaryRulesValidator.ValidateIntegerField(
               validatableNotifier.LanguageId,
               GeneralField.Language,
               CreateValidationRule(settings.Language));

        }

        private static ElementaryValidationRule CreateValidationRule(FieldProcessingSetting setting)
        {
            return new ElementaryValidationRule(!setting.Show, setting.Required);
        }

        #endregion
    }
}