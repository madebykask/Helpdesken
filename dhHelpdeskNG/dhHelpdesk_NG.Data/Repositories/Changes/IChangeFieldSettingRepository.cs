namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeFieldSettingRepository : INewRepository
    {
        ChangeEditSettings GetEnglishEditSettings(int customerId);

        ChangeEditSettings GetSwedishEditSettings(int customerId);

        void UpdateSettings(ChangeFieldSettings updatedSettings);

        ChangeOverviewSettings GetEnglishOverviewSettings(int customerId);

        ChangeOverviewSettings GetSwedishOverviewSettings(int customerId);

        ChangeFieldSettings GetEnglishFieldSettings(int customerId);
        
        ChangeFieldSettings GetSwedishFieldSettings(int customerId);

        SearchSettings GetEnglishSearchSettings(int customerId);

        SearchSettings GetSwedishSearchSettings(int customerId);

        ChangeProcessingSettings GetProcessingSettings(int customerId);
    }
}