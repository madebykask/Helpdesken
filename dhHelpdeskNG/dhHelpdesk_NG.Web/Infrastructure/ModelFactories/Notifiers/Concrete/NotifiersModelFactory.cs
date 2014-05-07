namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Common;
    using DH.Helpdesk.Web.Models.Notifiers;

    public sealed class NotifiersModelFactory : INotifiersModelFactory
    {
        private readonly INotifiersGridModelFactory notifiersGridModelFactory;

        public NotifiersModelFactory(INotifiersGridModelFactory notifiersGridModelFactory)
        {
            this.notifiersGridModelFactory = notifiersGridModelFactory;
        }

        public NotifiersModel Create(
            FieldSettings settings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchDivisions,
            NotifierFilters filters,
            SearchResult searchResult)
        {
            SearchDropDownModel domain;

            if (settings.Domain.ShowInNotifiers)
            {
                var items = searchDomains.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                var selectedValue = filters.DomainId.HasValue ? filters.DomainId.ToString() : null;
                var content = new DropDownContent(items, selectedValue);
                domain = new SearchDropDownModel(true, content);
            }
            else
            {
                domain = new SearchDropDownModel(false);
            }

            SearchDropDownModel region;
            SearchDropDownModel department;

            if (settings.Department.ShowInNotifiers)
            {
                var regionItems = searchRegions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();
                var departmentItems = searchDepartments.Select(d => new DropDownItem(d.Name, d.Value)).ToList();

                var regionSelectedValue = filters.RegionId.HasValue ? filters.RegionId.ToString() : null;
                var departmentSelectedValue = filters.DepartmentId.HasValue ? filters.DepartmentId.ToString() : null;

                var regionContent = new DropDownContent(regionItems, regionSelectedValue);
                region = new SearchDropDownModel(true, regionContent);

                var departmentContent = new DropDownContent(departmentItems, departmentSelectedValue);
                department = new SearchDropDownModel(true, departmentContent);
            }
            else
            {
                region = new SearchDropDownModel(false);
                department = new SearchDropDownModel(false);
            }

            SearchDropDownModel division;

            if (settings.Division.ShowInNotifiers)
            {
                var items = searchDivisions.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                var selectedValue = filters.DivisionId.HasValue ? filters.DivisionId.ToString() : null;
                var content = new DropDownContent(items, selectedValue);
                division = new SearchDropDownModel(true, content);
            }
            else
            {
                division = new SearchDropDownModel(false);
            }

            var sortField = new SortFieldModel { Name = filters.SortByField, SortBy = filters.SortBy };

            var searchModel = new SearchModel(
                domain,
                region,
                department,
                division,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage,
                sortField);

            var gridModel = this.notifiersGridModelFactory.Create(searchResult, settings, sortField);

            return new NotifiersModel(searchModel, gridModel);
        }

        public NotifiersModel CreateEmpty()
        {
            var empty = new NotifiersModel(
                    new SearchModel(
                        new SearchDropDownModel(false), 
                        new SearchDropDownModel(false),
                        new SearchDropDownModel(false),
                        new SearchDropDownModel(false),
                        string.Empty,
                        new NotifierStatus(), 
                        0,
                        new SortFieldModel()),
                    new NotifiersGridModel(
                        0,
                        new List<GridColumnHeaderModel>(),
                        new List<NotifierDetailedOverviewModel>(), 
                        new SortFieldModel()));
            empty.MarkAsEmpty();
            return empty;
        }
    }
}