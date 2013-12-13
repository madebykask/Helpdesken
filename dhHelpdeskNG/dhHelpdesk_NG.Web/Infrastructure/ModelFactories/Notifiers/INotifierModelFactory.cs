namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public interface INotifierModelFactory
    {
        NotifierModel Create(
            DisplayFieldsSettingsDto displaySettings,
            DisplayNotifierDto notifier,
            List<ItemOverviewDto> domains,
            List<ItemOverviewDto> regions,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> organizationUnits,
            List<ItemOverviewDto> divisions,
            List<ItemOverviewDto> managers,
            List<ItemOverviewDto> groups);
    }
}