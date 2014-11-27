namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Accounts
{
    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

    public interface IAccountValidator
    {
        void Validate(
            AccountForUpdate updatedDto,
            AccountForEdit existingDto,
            AccountFieldsSettingsForProcessing settings);

        void Validate(AccountForInsert newDto, AccountFieldsSettingsForProcessing settings);
    }

    public class AccountValidator : IAccountValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public AccountValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(
            AccountForUpdate updatedDto,
            AccountForEdit existingDto,
            AccountFieldsSettingsForProcessing settings)
        {
            throw new System.NotImplementedException();
        }

        public void Validate(AccountForInsert newDto, AccountFieldsSettingsForProcessing settings)
        {
            throw new System.NotImplementedException();
        }

        private void ValidateOrder(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.Id,
                existing.Orderer.Id,
                OrdererFields.Id,
                this.CreateValidationRule(settings.Orderer.Id));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.FirstName,
                existing.Orderer.FirstName,
                OrdererFields.FirstName,
                this.CreateValidationRule(settings.Orderer.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.LastName,
                existing.Orderer.LastName,
                OrdererFields.LastName,
                this.CreateValidationRule(settings.Orderer.LastName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.Phone,
                existing.Orderer.Phone,
                OrdererFields.Phone,
                this.CreateValidationRule(settings.Orderer.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.Email,
                existing.Orderer.Email,
                OrdererFields.Email,
                this.CreateValidationRule(settings.Orderer.Email));
        }

        private void ValidateUser(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            //this.elementaryRulesValidator.ValidateStringField(
            //    updated.User.Ids,
            //    existing.User.Ids,
            //    UserFields.Ids,
            //    this.CreateValidationRule(settings.User.Ids));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.FirstName,
                existing.User.FirstName,
                UserFields.FirstName,
                this.CreateValidationRule(settings.User.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Initials,
                existing.User.Initials,
                UserFields.Initials,
                this.CreateValidationRule(settings.User.Initials));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.LastName,
                existing.User.LastName,
                UserFields.LastName,
                this.CreateValidationRule(settings.User.LastName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.PersonalIdentityNumber,
                existing.User.PersonalIdentityNumber,
                UserFields.PersonalIdentityNumber,
                this.CreateValidationRule(settings.User.PersonalIdentityNumber));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Phone,
                existing.User.Phone,
                UserFields.Phone,
                this.CreateValidationRule(settings.User.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Extension,
                existing.User.Extension,
                UserFields.Extension,
                this.CreateValidationRule(settings.User.Extension));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.EMail,
                existing.User.EMail,
                UserFields.EMail,
                this.CreateValidationRule(settings.User.EMail));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Title,
                existing.User.Title,
                UserFields.Title,
                this.CreateValidationRule(settings.User.Title));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Location,
                existing.User.Location,
                UserFields.Location,
                this.CreateValidationRule(settings.User.Location));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.RoomNumber,
                existing.User.RoomNumber,
                UserFields.RoomNumber,
                this.CreateValidationRule(settings.User.RoomNumber));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.PostalAddress,
                existing.User.PostalAddress,
                UserFields.PostalAddress,
                this.CreateValidationRule(settings.User.PostalAddress));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.EmploymentType,
                existing.User.EmploymentType,
                UserFields.EmploymentType,
                this.CreateValidationRule(settings.User.EmploymentType));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.DepartmentId,
                existing.User.DepartmentId,
                UserFields.DepartmentId,
                this.CreateValidationRule(settings.User.DepartmentId));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.UnitId,
                existing.User.UnitId,
                UserFields.UnitId,
                this.CreateValidationRule(settings.User.UnitId));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.DepartmentId2,
                existing.User.DepartmentId2,
                UserFields.DepartmentId2,
                this.CreateValidationRule(settings.User.DepartmentId2));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Info,
                existing.User.Info,
                UserFields.Info,
                this.CreateValidationRule(settings.User.Info));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Responsibility,
                existing.User.Responsibility,
                UserFields.Responsibility,
                this.CreateValidationRule(settings.User.Responsibility));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Activity,
                existing.User.Activity,
                UserFields.Activity,
                this.CreateValidationRule(settings.User.Activity));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Manager,
                existing.User.Manager,
                UserFields.Manager,
                this.CreateValidationRule(settings.User.Manager));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.ReferenceNumber,
                existing.User.ReferenceNumber,
                UserFields.ReferenceNumber,
                this.CreateValidationRule(settings.User.ReferenceNumber));
        }



        private ElementaryValidationRule CreateValidationRule(FieldSetting setting)
        {
            return new ElementaryValidationRule(!setting.IsShowInDetails, setting.IsRequired);
        }
    }
}
