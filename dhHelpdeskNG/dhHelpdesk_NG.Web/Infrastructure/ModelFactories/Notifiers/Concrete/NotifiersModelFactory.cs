namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Infrastructure.FiltersExtractors.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.Session;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NotifiersModelFactory : INotifiersModelFactory
    {
        private readonly INotifiersGridModelFactory notifiersGridModelFactory;

        public NotifiersModelFactory(INotifiersGridModelFactory notifiersGridModelFactory)
        {
            this.notifiersGridModelFactory = notifiersGridModelFactory;
        }

        public NotifiersModel Create(
            FieldSettingsDto displaySettings,
            List<ItemOverviewDto> searchDomains,
            List<ItemOverviewDto> searchRegions,
            List<ItemOverviewDto> searchDepartments,
            List<ItemOverviewDto> searchDivisions,
            PageFilters predefinedFilters,
            Enums.Show showDefaultValue,
            int recordsOnPageDefaultValue,
            List<NotifierDetailedOverviewDto> notifiers)
        {
            SearchDropDownModel domain;

            if (displaySettings.Domain.ShowInNotifiers)
            {
                var items = searchDomains.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                string selectedValue = null;

                if (predefinedFilters != null)
                {
                    var predefinedFilter = predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.Domain);
                    selectedValue = predefinedFilter == null ? null : predefinedFilter.Value.ToString();
                }

                var content = new DropDownContent(items, selectedValue);
                domain = new SearchDropDownModel(true, content);
            }
            else
            {
                domain = new SearchDropDownModel(false);
            }

            SearchDropDownModel region;
            SearchDropDownModel department;

            if (displaySettings.Department.ShowInNotifiers)
            {
                var regionItems = searchRegions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();
                var departmentItems = searchDepartments.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                
                string regionSelectedValue = null;
                string departmentSelectedValue = null;

                if (predefinedFilters != null)
                {
                    var regionPredefinedFilter = predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.Region);

                    regionSelectedValue = regionPredefinedFilter == null
                                                  ? null
                                                  : regionPredefinedFilter.Value.ToString();

                    var departmentPredefinedFilter =
                        predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.Department);

                    departmentSelectedValue = departmentPredefinedFilter == null
                                                  ? null
                                                  : departmentPredefinedFilter.Value.ToString();
                }

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

            if (displaySettings.Division.ShowInNotifiers)
            {
                var items = searchDivisions.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                string selectedValue = null;

                if (predefinedFilters != null)
                {
                    var predefinedFilter = predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.Division);
                    selectedValue = predefinedFilter == null ? null : predefinedFilter.Value.ToString();
                }
                
                var content = new DropDownContent(items, selectedValue);
                division = new SearchDropDownModel(true, content);
            }
            else
            {
                division = new SearchDropDownModel(false);
            }

            string pharse = null;
            var show = showDefaultValue;
            var recordsOnPage = recordsOnPageDefaultValue;

            if (predefinedFilters != null)
            {
                var pharsePredefinedFilter = predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.Pharse);
                if (pharsePredefinedFilter != null)
                {
                    pharse = (string)pharsePredefinedFilter.Value;
                }

                var showPredefinedFilter = predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.Show);
                if (showPredefinedFilter != null)
                {
                    show = (Enums.Show)showPredefinedFilter.Value;
                }

                var recordsOnPagePredefinedFilter =
                    predefinedFilters.Filters.SingleOrDefault(f => f.Name == FilterName.RecordsOnPage);

                if (recordsOnPagePredefinedFilter != null)
                {
                    recordsOnPage = (int)recordsOnPagePredefinedFilter.Value;
                }
            }

            var searchModel = new SearchModel(domain, region, department, division, pharse, show, recordsOnPage);
            var gridModel = this.notifiersGridModelFactory.Create(notifiers, displaySettings);
            
            return new NotifiersModel(searchModel, gridModel);
        }
    }
}