namespace dhHelpdesk_NG.Service.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeService
    {
        SearchFieldSettingsDto FindSearchFieldSettings(int customerId);

        void UpdateSettings(UpdatedFieldSettingsDto updatedSettings);

        FieldSettingsDto FindSettings(int customerId, int languageId);

        List<ItemOverviewDto> FindStatusOverviews(int customerId);
            
        List<ItemOverviewDto> FindObjectOverviews(int customerId);
            
        List<ItemOverviewDto> FindActiveWorkingGroupOverviews(int customerId);
            
        List<ItemOverviewDto> FindActiveAdministratorOverviews(int customerId);

        Change FindChange(int changeId);

        void DeleteChange(int changeId);
            
            SearchResultDto SearchDetailedChangeOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            Data.Enums.Changes.ChangeStatus status,
            int selectCount);

        FieldOverviewSettingsDto FindFieldOverviewSettings(int customerId, int languageId);

        IDictionary<string, string> Validate(Change changeToValidate);

        IList<Change> GetChange(int customerId);
        IList<Change> GetChanges(int customerId);
        Change GetChange(int id, int customerId);

        void DeleteChange(Change change);
        void NewChange(Change change);
        void UpdateChange(Change change);
        void Commit();
    }
}