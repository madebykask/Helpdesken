namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Accounts
{
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;

    public interface IAccountValidator
    {
        void Validate(
            AccountForUpdate updatedDto,
            AccountForEdit existingDto,
            AccountFieldsSettingsForProcessing settings);

        void Validate(AccountForInsert newDto, AccountFieldsSettingsForProcessing settings);
    }
}
