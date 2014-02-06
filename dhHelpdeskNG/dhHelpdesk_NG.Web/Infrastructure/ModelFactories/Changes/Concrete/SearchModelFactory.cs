namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Common;

    public sealed class SearchModelFactory : ISearchModelFactory
    {
        public SearchModel Create(
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
            int recordsOnPage)
        {
            var statusesDropDown = CreateMultiSelectDropDown(searchFieldSettings.Status, statuses, selectedStatusIds);
            var objectsDropDown = CreateMultiSelectDropDown(searchFieldSettings.Object, objects, selectedObjectIds);
            
            var ownersDropDown = CreateMultiSelectDropDown(
                new FieldOverviewSetting(false, "gf"),
                new List<ItemOverviewDto>(0),
                new List<int>(0));

            var workingGroupsDropDown = CreateMultiSelectDropDown(
                searchFieldSettings.WorkingGroup,
                workingGroups,
                selectedWorkingGroupIds);

            var administratorsDropDown = CreateMultiSelectDropDown(
                searchFieldSettings.Administrator,
                administrators,
                selectedAdministratorIds);

            var showList = CreateShowSelectList();

            return new SearchModel(
                statusesDropDown,
                objectsDropDown,
                ownersDropDown,
                workingGroupsDropDown,
                administratorsDropDown,
                pharse,
                showList,
                recordsOnPage);
        }

        private static SearchDropDownModel<MultiSelectList> CreateMultiSelectDropDown(
            FieldOverviewSetting fieldSetting,
            List<ItemOverviewDto> items,
            List<int> selectedItemIds)
        {
            if (!fieldSetting.Show)
            {
                return new SearchDropDownModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedItemIds);
            return new SearchDropDownModel<MultiSelectList>(true, fieldSetting.Caption, list);
        }

        private static SelectList CreateShowSelectList()
        {
            var activeItem = new SelectListItem();
            activeItem.Text = Translation.Get("Active", Enums.TranslationSource.TextTranslation);
            activeItem.Value = ChangeStatus.Active.ToString();

            var finishedItem = new SelectListItem();
            finishedItem.Text = Translation.Get("Finished", Enums.TranslationSource.TextTranslation);
            finishedItem.Value = ChangeStatus.Finished.ToString();

            var noneItem = new SelectListItem();
            noneItem.Text = Translation.Get("None", Enums.TranslationSource.TextTranslation);
            noneItem.Value = ChangeStatus.None.ToString();

            var showItems = new List<SelectListItem> { activeItem, finishedItem, noneItem };
            return new SelectList(showItems, "Value", "Text");
        }
    }
}