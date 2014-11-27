namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Account
{
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;

    public interface IAccountRestorer
    {
        void Restore(AccountForUpdate dto, AccountForEdit existingDto, AccountFieldsSettingsForProcessing settings);
    }
}
