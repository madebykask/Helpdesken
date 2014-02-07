namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class NotifiersModelFactory : INotifiersModelFactory
    {
        private readonly INotifiersGridModelFactory notifiersGridModelFactory;

        public NotifiersModelFactory(INotifiersGridModelFactory notifiersGridModelFactory)
        {
            this.notifiersGridModelFactory = notifiersGridModelFactory;
        }

        public NotifiersModel Create(
            FieldSettingsDto displaySettings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchDivisions,
            NotifierFilters predefinedFilters,
            Enums.Show showDefaultValue,
            int recordsOnPageDefaultValue,
            SearchResultDto searchResult)
        {
            SearchDropDownModel domain;

            if (displaySettings.Domain.ShowInNotifiers)
            {
                var items = searchDomains.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                string selectedValue = null;

                if (predefinedFilters != null)
                {
                    selectedValue = predefinedFilters.DomainId.HasValue ? predefinedFilters.DomainId.ToString() : null;
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
                    regionSelectedValue = predefinedFilters.RegionId.HasValue
                                              ? predefinedFilters.RegionId.ToString()
                                              : null;

                    departmentSelectedValue = predefinedFilters.DepartmentId.HasValue
                                                  ? predefinedFilters.DepartmentId.ToString()
                                                  : null;
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
                    selectedValue = predefinedFilters.DivisionId.HasValue
                                        ? predefinedFilters.DivisionId.ToString()
                                        : null;
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
                pharse = predefinedFilters.Pharse;
                show = predefinedFilters.Show;
                recordsOnPage = predefinedFilters.RecordsOnPage;
            }

            var searchModel = new SearchModel(domain, region, department, division, pharse, show, recordsOnPage);
            var gridModel = this.notifiersGridModelFactory.Create(searchResult, displaySettings);
            
            return new NotifiersModel(searchModel, gridModel);
        }
    }
}