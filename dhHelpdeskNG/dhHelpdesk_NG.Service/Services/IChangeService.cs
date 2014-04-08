namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Domain.Changes;

    using NewChangeRequest = DH.Helpdesk.Services.Requests.Changes.NewChangeRequest;
    using UpdateChangeRequest = DH.Helpdesk.Services.Requests.Changes.UpdateChangeRequest;

    public interface IChangeService
    {
        #region Public Methods and Operators

        ExcelFile ExportChangesToExcelFile(SearchParameters parameters, int languageId);

        void AddChange(NewChangeRequest request);

        void DeleteChange(int changeId);

        bool FileExists(int changeId, Subtopic subtopic, string fileName);

        FindChangeResponse FindChange(int changeId);

        List<string> FindFileNames(int changeId, Subtopic subtopic, List<string> excludeFiles);

        List<Log> FindLogs(int changeId, Subtopic subtopic, List<int> excludeLogIds);

        ChangeFieldSettings FindSettings(int customerId, int languageId);

        SearchSettings GetSearchSettings(int customerId, int languageId);

        SearchData GetSearchData(int customerId);

        ChangeEditData GetChangeEditData(int changeId, int customerId, ChangeEditSettings settings);

        ChangeEditSettings GetChangeEditSettings(int customerId, int languageId);

        ChangeOverviewSettings GetChangeOverviewSettings(int customerId, int languageId);

        IList<ChangeEntity> GetChanges(int customerId);

        byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName);

        ChangeEditData GetNewChangeEditData(int customerId, ChangeEditSettings settings);

        SearchResult Search(SearchParameters parameters);

        void UpdateChange(UpdateChangeRequest request);

        void UpdateSettings(ChangeFieldSettings settings);

        #endregion
    }
}