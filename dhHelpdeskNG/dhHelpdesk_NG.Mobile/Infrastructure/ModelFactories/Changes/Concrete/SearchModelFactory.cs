namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Mobile.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Mobile.Models.Changes;
    using DH.Helpdesk.Mobile.Models.Shared;

    public sealed class SearchModelFactory : ISearchModelFactory
    {
        #region Public Methods and Operators

        public SearchModel Create(ChangesFilter filter, GetSearchDataResponse response)
        {
            var statuses = CreateMultiSelectField(
                response.OverviewSettings.Statuses,
                response.SearchOptions.Statuses,
                filter.StatusIds);

            var objects = CreateMultiSelectField(
                response.OverviewSettings.Objects,
                response.SearchOptions.Objects,
                filter.ObjectIds);

            var owners = CreateMultiSelectField(
                response.OverviewSettings.Owners,
                response.SearchOptions.Owners,
                filter.OwnerIds);

            var affectedProcesses = CreateMultiSelectField(
                response.OverviewSettings.AffectedProcesses,
                response.SearchOptions.AffectedProcesses,
                filter.AffectedProcessIds);

            var workingGroups = CreateMultiSelectField(
                response.OverviewSettings.WorkingGroups,
                response.SearchOptions.WorkingGroups,
                filter.WorkingGroupIds);

            var administrators = CreateMultiSelectField(
                response.OverviewSettings.Administrators,
                response.SearchOptions.Administrators,
                filter.AdministratorIds);

            var show = CreateShowSelectList(filter.Status);

            SortFieldModel sortField = null;

            if (filter.SortField != null)
            {
                sortField = new SortFieldModel { Name = filter.SortField.Name, SortBy = filter.SortField.SortBy };
            }

            return new SearchModel(
                statuses,
                objects,
                owners,
                affectedProcesses,
                workingGroups,
                administrators,
                filter.Pharse,
                show,
                filter.RecordsOnPage,
                sortField);
        }

        #endregion

        #region Methods

        private static ConfigurableSearchFieldModel<MultiSelectList> CreateMultiSelectField(
            FieldOverviewSetting setting,
            List<ItemOverview> items,
            List<int> selectedIds)
        {
            if (!setting.Show)
            {
                return ConfigurableSearchFieldModel<MultiSelectList>.CreateUnshowable();
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedIds);
            return new ConfigurableSearchFieldModel<MultiSelectList>(setting.Caption, list);
        }

        private static SelectList CreateShowSelectList(ChangeStatus? status)
        {
            var activeItem = new SelectListItem();
            activeItem.Text = Translation.Get("Pågående ändringar", Enums.TranslationSource.TextTranslation);
            activeItem.Value = ChangeStatus.Active.ToString();

            var finishedItem = new SelectListItem();
            finishedItem.Text = Translation.Get("Avslutade ändringar", Enums.TranslationSource.TextTranslation);
            finishedItem.Value = ChangeStatus.Finished.ToString();

            var noneItem = new SelectListItem();
            noneItem.Text = Translation.Get("Alla", Enums.TranslationSource.TextTranslation);

            var showItems = new List<SelectListItem> { activeItem, finishedItem, noneItem };
            return new SelectList(showItems, "Value", "Text", status);
        }

        #endregion
    }
}