namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit;

    public interface IChangeFieldSettingRepository : IRepository<ChangeFieldSettingsEntity>
    {
        ChangeEditSettings FindChangeEditSettings(int customerId, int languageId);

        FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSettings(UpdatedFieldSettingsDto updatedSettings);

        FieldOverviewSettings FindEnglishByCustomerId(int customerId);

        FieldOverviewSettings FindSwedishByCustomerId(int customerId);
    }
}