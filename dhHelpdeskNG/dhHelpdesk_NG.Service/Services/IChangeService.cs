namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeService
    {
        ChangeEditSettings FindChangeEditSettings(int customerId, int languageId);

        List<Log> FindLogs(int changeId, Subtopic subtopic, List<int> excludeIds);

        void DeleteFile(int changeId, Subtopic subtopic, string fileName);

        List<string> FindFileNames(int changeId, Subtopic subtopic);

        List<string> FindFileNames(int changeId, Subtopic subtopic, List<string> excludeFiles); 

        void AddFile(NewFile file);

        bool FileExists(int changeId, Subtopic subtopic, string fileName);

        byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName);

        void AddChange(NewChangeAggregate newChange);

        void UpdateChange(UpdatedChangeAggregate updatedChange);

        SearchFieldSettings FindSearchFieldSettings(int customerId);

        void UpdateSettings(UpdatedFieldSettingsDto updatedSettings);

        FieldSettings FindSettings(int customerId, int languageId);

        List<ItemOverview> FindStatusOverviews(int customerId);
            
        List<ItemOverview> FindObjectOverviews(int customerId);
            
        List<ItemOverview> FindActiveWorkingGroupOverviews(int customerId);
            
        List<ItemOverview> FindActiveAdministratorOverviews(int customerId);

        ChangeAggregate FindChange(int changeId);

        ChangeEditOptions FindChangeOptionalData(int customerId, int changeId, ChangeEditSettings editSettings);

        ChangeEditOptions FindNewChangeOptionalData(int customerId);

        void DeleteChange(int changeId);

        SearchResultDto SearchDetailedChangeOverviews(SearchParameters parameters);

        FieldOverviewSettings FindFieldOverviewSettings(int customerId, int languageId);

        IList<ChangeEntity> GetChanges(int customerId);
    }
}