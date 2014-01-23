namespace dhHelpdesk_NG.Service.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeService
    {
        void UpdateChange(UpdatedChangeAggregate updatedChange);

        SearchFieldSettingsDto FindSearchFieldSettings(int customerId);

        void UpdateSettings(UpdatedFieldSettingsDto updatedSettings);

        FieldSettingsDto FindSettings(int customerId, int languageId);

        List<ItemOverviewDto> FindStatusOverviews(int customerId);
            
        List<ItemOverviewDto> FindObjectOverviews(int customerId);
            
        List<ItemOverviewDto> FindActiveWorkingGroupOverviews(int customerId);
            
        List<ItemOverviewDto> FindActiveAdministratorOverviews(int customerId);

        ChangeAggregate FindChange(int changeId);

        ChangeOptionalData FindChangeOptionalData(int customerId);

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

        IList<ChangeEntity> GetChanges(int customerId);

    }
}