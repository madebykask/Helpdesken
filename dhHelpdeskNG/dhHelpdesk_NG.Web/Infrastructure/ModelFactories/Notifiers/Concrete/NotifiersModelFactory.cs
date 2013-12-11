namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

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

        public NotifiersModel Create(FieldsSettingsDto fieldsSettings, List<DomainOverviewDto> searchDomains, List<DepartmentOverviewDto> searchDepartments, List<DivisionOverviewDto> searchDivisions, Enums.Show show, int recordsOnPage, List<NotifierDetailedOverviewDto> notifiers)
        {
            SearchDropDownModel domain;

            if (fieldsSettings.Domain.ShowInNotifiers)
            {
                var items =
                    searchDomains.Select(d => new DropDownItem(d.Name, d.Id.ToString(CultureInfo.InvariantCulture)))
                                 .ToList();

                var content = new DropDownContent(items);
                domain = new SearchDropDownModel(true, content);
            }
            else
            {
                domain = new SearchDropDownModel(false);
            }

            SearchDropDownModel department;

            if (fieldsSettings.Department.ShowInNotifiers)
            {
                var items =
                    searchDepartments.Select(d => new DropDownItem(d.Name, d.Id.ToString(CultureInfo.InvariantCulture)))
                                 .ToList();

                var content = new DropDownContent(items);
                department = new SearchDropDownModel(true, content);
            }
            else
            {
                department = new SearchDropDownModel(false);
            }

            SearchDropDownModel division;

            if (fieldsSettings.Division.ShowInNotifiers)
            {
                var items =
                    searchDivisions.Select(d => new DropDownItem(d.Name, d.Id.ToString(CultureInfo.InvariantCulture)))
                                 .ToList();

                var content = new DropDownContent(items);
                division = new SearchDropDownModel(true, content);
            }
            else
            {
                division = new SearchDropDownModel(false);
            }

            var searchModel = new SearchModel(
                domain,
                department,
                division,
                show,
                recordsOnPage);

            var gridModel = this.notifiersGridModelFactory.Create(notifiers, fieldsSettings);
            return new NotifiersModel(searchModel, gridModel);
        }
    }
}