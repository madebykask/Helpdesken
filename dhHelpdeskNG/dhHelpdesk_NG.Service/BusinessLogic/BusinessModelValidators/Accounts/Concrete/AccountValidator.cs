namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Accounts.Concrete
{
    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

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
            this.ValidateOrder(newDto, settings);
            this.ValidateUser(newDto, settings);
            this.ValidateAccountInformation(newDto, settings);
            this.ValidateDelivery(newDto, settings);
            this.ValidateProgram(newDto, settings);
            this.ValidateOther(newDto, settings);
        }

        private void ValidateOrder(AccountForInsert updated, AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.Id,
                OrdererFields.Id,
                this.CreateValidationRule(settings.Orderer.Id));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.FirstName,
                OrdererFields.FirstName,
                this.CreateValidationRule(settings.Orderer.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.LastName,
                OrdererFields.LastName,
                this.CreateValidationRule(settings.Orderer.LastName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.Phone,
                OrdererFields.Phone,
                this.CreateValidationRule(settings.Orderer.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Orderer.Email,
                OrdererFields.Email,
                this.CreateValidationRule(settings.Orderer.Email));
        }

        private void ValidateUser(AccountForInsert updated, AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateForNew(
                updated.User.Ids,
                UserFields.Ids,
                this.CreateValidationRule(settings.User.Ids));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.FirstName,
                UserFields.FirstName,
                this.CreateValidationRule(settings.User.FirstName));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Initials,
                UserFields.Initials,
                this.CreateValidationRule(settings.User.Initials));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.LastName,
                UserFields.LastName,
                this.CreateValidationRule(settings.User.LastName));

            this.elementaryRulesValidator.ValidateForNew(
                updated.User.PersonalIdentityNumber,
                UserFields.PersonalIdentityNumber,
                this.CreateValidationRule(settings.User.PersonalIdentityNumber));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Phone,
                UserFields.Phone,
                this.CreateValidationRule(settings.User.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Extension,
                UserFields.Extension,
                this.CreateValidationRule(settings.User.Extension));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.EMail,
                UserFields.EMail,
                this.CreateValidationRule(settings.User.EMail));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Title,
                UserFields.Title,
                this.CreateValidationRule(settings.User.Title));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Location,
                UserFields.Location,
                this.CreateValidationRule(settings.User.Location));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.RoomNumber,
                UserFields.RoomNumber,
                this.CreateValidationRule(settings.User.RoomNumber));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.PostalAddress,
                UserFields.PostalAddress,
                this.CreateValidationRule(settings.User.PostalAddress));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.EmploymentType,
                UserFields.EmploymentType,
                this.CreateValidationRule(settings.User.EmploymentType));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.DepartmentId,
                UserFields.DepartmentId,
                this.CreateValidationRule(settings.User.DepartmentId));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.UnitId,
                UserFields.UnitId,
                this.CreateValidationRule(settings.User.UnitId));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.User.DepartmentId2,
                UserFields.DepartmentId2,
                this.CreateValidationRule(settings.User.DepartmentId2));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Info,
                UserFields.Info,
                this.CreateValidationRule(settings.User.Info));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Responsibility,
                UserFields.Responsibility,
                this.CreateValidationRule(settings.User.Responsibility));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Activity,
                UserFields.Activity,
                this.CreateValidationRule(settings.User.Activity));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.Manager,
                UserFields.Manager,
                this.CreateValidationRule(settings.User.Manager));

            this.elementaryRulesValidator.ValidateStringField(
                updated.User.ReferenceNumber,
                UserFields.ReferenceNumber,
                this.CreateValidationRule(settings.User.ReferenceNumber));
        }

        private void ValidateAccountInformation(AccountForInsert updated, AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.AccountInformation.StartedDate,
                AccountInformationFields.StartedDate,
                this.CreateValidationRule(settings.AccountInformation.StartedDate));

            this.elementaryRulesValidator.ValidateDateTimeField(
                updated.AccountInformation.FinishDate,
                AccountInformationFields.FinishDate,
                this.CreateValidationRule(settings.AccountInformation.FinishDate));

            this.elementaryRulesValidator.ValidateIntegerField(
                (int)updated.AccountInformation.EMailTypeId,
                AccountInformationFields.EMailTypeId,
                this.CreateValidationRule(settings.AccountInformation.EMailTypeId));

            this.elementaryRulesValidator.ValidateBooleanField(
                updated.AccountInformation.HomeDirectory,
                AccountInformationFields.HomeDirectory,
                this.CreateValidationRule(settings.AccountInformation.HomeDirectory));

            this.elementaryRulesValidator.ValidateBooleanField(
                updated.AccountInformation.Profile,
                AccountInformationFields.Profile,
                this.CreateValidationRule(settings.AccountInformation.Profile));

            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountInformation.InventoryNumber,
                AccountInformationFields.InventoryNumber,
                this.CreateValidationRule(settings.AccountInformation.InventoryNumber));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.AccountInformation.AccountTypeId,
                AccountInformationFields.AccountTypeId,
                this.CreateValidationRule(settings.AccountInformation.AccountTypeId));

            this.elementaryRulesValidator.ValidateForNew(
                updated.AccountInformation.AccountType2,
                AccountInformationFields.AccountType2,
                this.CreateValidationRule(settings.AccountInformation.AccountType2));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.AccountInformation.AccountType3,
                AccountInformationFields.AccountType3,
                this.CreateValidationRule(settings.AccountInformation.AccountType3));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.AccountInformation.AccountType4,
                AccountInformationFields.AccountType4,
                this.CreateValidationRule(settings.AccountInformation.AccountType4));

            this.elementaryRulesValidator.ValidateIntegerField(
                updated.AccountInformation.AccountType5,
                AccountInformationFields.AccountType5,
                this.CreateValidationRule(settings.AccountInformation.AccountType5));

            this.elementaryRulesValidator.ValidateStringField(
                updated.AccountInformation.Info,
                AccountInformationFields.Info,
                this.CreateValidationRule(settings.AccountInformation.Info));
        }

        private void ValidateDelivery(AccountForInsert updated, AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.DeliveryInformation.Name,
                DeliveryInformationFields.Name,
                this.CreateValidationRule(settings.DeliveryInformation.Name));

            this.elementaryRulesValidator.ValidateStringField(
                updated.DeliveryInformation.Phone,
                DeliveryInformationFields.Phone,
                this.CreateValidationRule(settings.DeliveryInformation.Phone));

            this.elementaryRulesValidator.ValidateStringField(
                updated.DeliveryInformation.Address,
                DeliveryInformationFields.Address,
                this.CreateValidationRule(settings.DeliveryInformation.Address));

            this.elementaryRulesValidator.ValidateStringField(
                updated.DeliveryInformation.PostalAddress,
                DeliveryInformationFields.PostalAddress,
                this.CreateValidationRule(settings.DeliveryInformation.PostalAddress));
        }

        private void ValidateProgram(
            AccountForInsert updated,
            AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateForNew(
                updated.Program.ProgramIds,
                ProgramFields.Programs,
                this.CreateValidationRule(settings.Program.Programs));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Program.InfoProduct,
                ProgramFields.InfoProduct,
                this.CreateValidationRule(settings.Program.InfoProduct));
        }

        private void ValidateOther(AccountForInsert updated, AccountFieldsSettingsForProcessing settings)
        {
            this.elementaryRulesValidator.ValidateStringField(
                updated.Other.Info,
                OtherFields.Info,
                this.CreateValidationRule(settings.Other.Info));

            this.elementaryRulesValidator.ValidateRealField(
                updated.Other.CaseNumber,
                OtherFields.CaseNumber,
                this.CreateValidationRule(settings.Other.CaseNumber));

            this.elementaryRulesValidator.ValidateStringField(
                updated.Other.FileName,
                OtherFields.FileName,
                this.CreateValidationRule(settings.Other.FileName));

            this.elementaryRulesValidator.ValidateForNew(
                updated.Other.Content,
                OtherFields.FileName,
                this.CreateValidationRule(settings.Other.FileName));
        }

        private ElementaryValidationRule CreateValidationRule(FieldSetting setting)
        {
            return new ElementaryValidationRule(!setting.IsShowInDetails, setting.IsRequired);
        }
    }
}