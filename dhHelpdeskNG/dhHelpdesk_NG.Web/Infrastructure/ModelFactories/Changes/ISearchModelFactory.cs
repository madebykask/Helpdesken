namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;

    public interface ISearchModelFactory
    {
        SearchModel Create(
            SearchFieldSettings searchFieldSettings, 
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