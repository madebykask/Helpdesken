namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface ISearchModelFactory
    {
        SearchModel Create(
            SearchFieldSettingsDto searchFieldSettings, 
            List<ItemOverviewDto> statuses,
            List<int> selectedStatusIds,
            List<ItemOverviewDto> objects,
            List<int> selectedObjectIds,
            List<ItemOverviewDto> workingGroups,
            List<int> selectedWorkingGroupIds,
            List<ItemOverviewDto> administrators,
            List<int> selectedAdministratorIds,
            ChangeStatus status,
            string pharse,
            int recordsOnPage);
    }
}