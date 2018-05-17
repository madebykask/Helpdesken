namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.Response.Changes;

    using NewChangeRequest = DH.Helpdesk.Services.Requests.Changes.NewChangeRequest;
    using UpdateChangeRequest = DH.Helpdesk.Services.Requests.Changes.UpdateChangeRequest;

    public interface IChangeService
    {
        #region Public Methods and Operators

        ExcelFile ExportChangesToExcelFile(SearchParameters parameters, OperationContext context);

        void AddChange(NewChangeRequest request);

        void DeleteChange(int changeId);

        bool FileExists(int changeId, Subtopic subtopic, string fileName);

        FindChangeResponse FindChange(int changeId, OperationContext context);

        List<string> FindChangeFileNamesExcludeDeleted(int changeId, Subtopic subtopic, List<string> excludeFiles);

        List<Log> FindChangeLogsExcludeSpecified(int changeId, Subtopic subtopic, List<int> excludeLogIds);

        GetSettingsResponse GetSettings(int languageId, OperationContext context);

        GetSearchDataResponse GetSearchData(OperationContext context);

        SearchSettings GetSearchSettings(int customerId, int languageId);

        ChangeEditOptions GetChangeEditData(int changeId, ChangeEditSettings settings, OperationContext context);

        ChangeEditSettings GetChangeEditSettings(int customerId, int languageId);

        GetNewChangeEditDataResponse GetNewChangeEditData(OperationContext context);

        ChangeOverviewSettings GetChangeOverviewSettings(int customerId, int languageId, bool onlyListSettings);

        IList<ChangeOverview> GetChanges(int customerId);

        byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName);

        SearchResponse Search(SearchParameters parameters, OperationContext context);

        void UpdateChange(UpdateChangeRequest request);

        void UpdateSettings(ChangeFieldSettings settings);

        ChangeOverview GetChangeOverview(int id);

        List<CustomerChange> GetCustomersChanges(int[] customersIds);

        CustomerChanges[] GetCustomerChanges(int[] customerIds, int userId);

        #endregion
    }
}