namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
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
            Enums.Show show,
            int recordsOnPage,
            List<NotifierDetailedOverviewDto> notifiers)
        {
            SearchDropDownModel domain;

            if (displaySettings.Domain.ShowInNotifiers)
            {
                var items = searchDomains.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                var content = new DropDownContent(items);
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
                var regionContent = new DropDownContent(regionItems);
                region = new SearchDropDownModel(true, regionContent);

                var departmentItems = searchDepartments.Select(d => new DropDownItem(d.Name, d.Value)).ToList();
                var departmentContent = new DropDownContent(departmentItems);
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
                var content = new DropDownContent(items);
                division = new SearchDropDownModel(true, content);
            }
            else
            {
                division = new SearchDropDownModel(false);
            }

            var searchModel = new SearchModel(domain, region, department, division, show, recordsOnPage);
            var gridModel = this.notifiersGridModelFactory.Create(notifiers, displaySettings);
            
            return new NotifiersModel(searchModel, gridModel);
        }
    }
}