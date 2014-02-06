namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;

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

        List<ItemOverviewDto> FindStatusOverviews(int customerId);
            
        List<ItemOverviewDto> FindObjectOverviews(int customerId);
            
        List<ItemOverviewDto> FindActiveWorkingGroupOverviews(int customerId);
            
        List<ItemOverviewDto> FindActiveAdministratorOverviews(int customerId);

        ChangeAggregate FindChange(int changeId);

        ChangeEditOptions FindChangeOptionalData(int customerId, int changeId, ChangeEditSettings editSettings);

        ChangeEditOptions FindNewChangeOptionalData(int customerId);

        void DeleteChange(int changeId);

        SearchResultDto SearchDetailedChangeOverviews(SearchParameters parameters);

        FieldOverviewSettings FindFieldOverviewSettings(int customerId, int languageId);

        IList<ChangeEntity> GetChanges(int customerId);
    }
}