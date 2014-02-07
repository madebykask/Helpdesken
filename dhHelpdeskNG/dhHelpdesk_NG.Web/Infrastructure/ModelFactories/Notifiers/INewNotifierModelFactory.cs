namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INewNotifierModelFactory
    {
        NotifierInputModel Create(
            DisplayFieldSettingsDto displaySettings,
            List<ItemOverviewDto> domains,
            List<ItemOverviewDto> regions,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> organizationUnits,
            List<ItemOverviewDto> divisions,
            List<ItemOverviewDto> managers,
            List<ItemOverviewDto> groups);
    }
}