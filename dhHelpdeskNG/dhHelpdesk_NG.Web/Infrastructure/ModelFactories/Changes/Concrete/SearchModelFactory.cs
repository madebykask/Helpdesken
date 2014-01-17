namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class SearchModelFactory : ISearchModelFactory
    {
        public SearchModel Create(
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
            int recordsOnPage)
        {
            var statusItems = searchFieldSettings.Statuses.Show
                                  ? new MultiSelectList(statuses, "Value", "Name", selectedStatusIds)
                                  : new MultiSelectList(new List<object>(0));

            var objectItems = searchFieldSettings.Objects.Show
                                  ? new MultiSelectList(objects, "Value", "Name", selectedObjectIds)
                                  : new MultiSelectList(new List<object>(0));

            var workingGroupItems = searchFieldSettings.WorkingGroups.Show
                                        ? new MultiSelectList(workingGroups, "Value", "Name", selectedWorkingGroupIds)
                                        : new MultiSelectList(new List<object>(0));

            var administratorItems = searchFieldSettings.Administrators.Show
                                         ? new MultiSelectList(
                                               administrators, "Value", "Name", selectedAdministratorIds)
                                         : new MultiSelectList(new List<object>(0));

            var showItems = new List<object>
                            {
                                new { Name = Translation.Get("Active", Enums.TranslationSource.TextTranslation), Value = ChangeStatus.Active },
                                new { Name = Translation.Get("Finished", Enums.TranslationSource.TextTranslation), Value = ChangeStatus.Finished },
                                new { Name = Translation.Get("None", Enums.TranslationSource.TextTranslation), Value = ChangeStatus.None }
                            };

            var showList = new SelectList(showItems, "Value", "Name");

            return new SearchModel(
                statusItems,
                objectItems,
                workingGroupItems,
                administratorItems,
                pharse,
                showList,
                recordsOnPage);
        }
    }
}