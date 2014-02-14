namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeFieldSettingRepository : INewRepository
    {
        ChangeEditSettings GetEnglishEditSettings(int customerId);

        ChangeEditSettings GetSwedishEditSettings(int customerId);

        void UpdateSettings(UpdatedSettings updatedSettings);

        ChangeOverviewSettings GetEnglishOverviewSettings(int customerId);

        ChangeOverviewSettings GetSwedishOverviewSettings(int customerId);

        ChangeFieldSettings GetEnglishFieldSettings(int customerId);
        
        ChangeFieldSettings GetSwedishFieldSettings(int customerId);
    }
}