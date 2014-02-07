namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeFieldSettingRepository : IRepository<ChangeFieldSettingsEntity>
    {
        ChangeEditSettings FindChangeEditSettings(int customerId, int languageId);

        FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId);

        void UpdateSettings(UpdatedFieldSettingsDto updatedSettings);

        FieldOverviewSettings FindEnglishByCustomerId(int customerId);

        FieldOverviewSettings FindSwedishByCustomerId(int customerId);
    }
}