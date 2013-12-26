namespace dhHelpdesk_NG.Data.Repositories
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeFieldSettingRepository : IRepository<ChangeFieldSettings>
    {
        FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSettings(UpdatedFieldSettingsDto updatedSettings);
    }
}