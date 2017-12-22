namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Shared;

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
            List<ItemOverview> searchOrganizationUnit,
            List<ItemOverview> searchDivisions,
			List<ItemOverview> searchComputerUserCategories,
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
            SearchDropDownModel organizationUnit;

            if (settings.Region.ShowInNotifiers)
            {
                var regionItems = searchRegions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();

                var regionSelectedValue = filters.RegionId.HasValue ? filters.RegionId.ToString() : null;

                var regionContent = new DropDownContent(regionItems, regionSelectedValue);

                region = new SearchDropDownModel(true, regionContent);
            }
            else
            {
                region = new SearchDropDownModel(false);
            }

            if (settings.Department.ShowInNotifiers)
            {
                var regionItems = searchRegions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();
                var departmentItems = searchDepartments.Select(d => new DropDownItem(d.Name, d.Value)).ToList();

                var regionSelectedValue = filters.RegionId.HasValue ? filters.RegionId.ToString() : null;
                var departmentSelectedValue = filters.DepartmentId.HasValue ? filters.DepartmentId.ToString() : null;

                var regionContent = new DropDownContent(regionItems, regionSelectedValue);
                //region = new SearchDropDownModel(true, regionContent);

                var departmentContent = new DropDownContent(departmentItems, departmentSelectedValue);
                department = new SearchDropDownModel(true, departmentContent);

                if (settings.OrganizationUnit.ShowInNotifiers)
                {
                    var organizationUnitItems = searchOrganizationUnit.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                    var organizationUnitSelectedValue = filters.OrganizationUnitId.HasValue ? filters.OrganizationUnitId.ToString() : null;
                    var organizationUnitContent = new DropDownContent(organizationUnitItems, organizationUnitSelectedValue);
                    organizationUnit = new SearchDropDownModel(true, organizationUnitContent);
                }
                else
                {
                    organizationUnit = new SearchDropDownModel(false);
                }
            }
            else
            {

                department = new SearchDropDownModel(false);
                organizationUnit = new SearchDropDownModel(false);
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

			SearchDropDownModel computerUserCategories;
			if (searchComputerUserCategories != null && searchComputerUserCategories.Any())
			{
				var items = searchComputerUserCategories.Select(d => new DropDownItem(d.Name, d.Value)).ToList();

				// TODO: Translation
				//items.Insert(0, new DropDownItem("Employee", "0"));
				var content = new DropDownContent(items, "0");
				computerUserCategories = new SearchDropDownModel(true, content);
			}
			else
			{
				computerUserCategories = new SearchDropDownModel(false);
			}

			var searchModel = new SearchModel(
                domain,
                region,
                department,
                organizationUnit,
                division,
				computerUserCategories,
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
            
            return empty;
        }
    }
}