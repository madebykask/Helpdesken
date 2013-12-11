namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public interface INewNotifierModelFactory
    {
        NotifierInputModel Create(
            DisplayFieldsSettingsDto displaySettings,
            List<DomainOverviewDto> domains,
            List<DepartmentOverviewDto> departments,
            List<OrganizationUnitOverviewDto> organizationUnits,
            List<DivisionOverviewDto> divisions,
            List<NotifierOverviewDto> managers,
            List<NotifierGroupOverviewDto> groups);
    }
}