namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public interface IIndexModelFactory
    {
        IndexModel Create(FieldsSettingsDto fieldsSettings, List<DomainOverviewDto> searchDomains, List<DepartmentOverviewDto> searchDepartments, List<DivisionOverviewDto> searchDivisions, Enums.Show show, int recordsOnPage, List<NotifierDetailedOverviewDto> notifiers);
    }
}