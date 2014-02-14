namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class SearchModelFactory : ISearchModelFactory
    {
        #region Public Methods and Operators

        public SearchModel Create(ChangesFilter filter, SearchData searchData, SearchSettings searchSettings)
        {
            var statusList = CreateMultiSelectField(searchSettings.Statuses, searchData.Statuses, filter.StatusIds);
            var objectList = CreateMultiSelectField(searchSettings.Objects, searchData.Objects, filter.ObjectIds);
            var ownerList = CreateMultiSelectField(searchSettings.Owners, searchData.Owners, filter.OwnerIds);

            var affectedProcessList = CreateMultiSelectField(
                searchSettings.AffectedProcesses,
                searchData.AffectedProcesses,
                filter.AffectedProcessIds);

            var workingGroupList = CreateMultiSelectField(
                searchSettings.WorkingGroups,
                searchData.WorkingGroups,
                filter.WorkingGroupIds);

            var administratorList = CreateMultiSelectField(
                searchSettings.Administrators,
                searchData.Administrators,
                filter.AdministratorIds);

            var showList = CreateShowSelectList();

            return new SearchModel(
                statusList,
                objectList,
                ownerList,
                affectedProcessList,
                workingGroupList,
                administratorList,
                filter.Pharse,
                showList,
                filter.RecordsOnPage);
        }

        #endregion

        #region Methods

        private static ConfigurableSearchFieldModel<MultiSelectList> CreateMultiSelectField(
            FieldOverviewSetting overviewSetting,
            List<ItemOverview> items,
            List<int> selectedIds)
        {
            if (!overviewSetting.Show)
            {
                return new ConfigurableSearchFieldModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedIds);
            return new ConfigurableSearchFieldModel<MultiSelectList>(true, overviewSetting.Caption, list);
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
            noneItem.Value = ChangeStatus.Active.ToString();

            var showItems = new List<SelectListItem> { activeItem, finishedItem, noneItem };
            return new SelectList(showItems, "Value", "Text");
        }

        #endregion
    }
}