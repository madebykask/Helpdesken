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
            List<ItemOverview> domains,
            List<ItemOverview> regions,
            List<ItemOverview> departments,
            List<ItemOverview> organizationUnits,
            List<ItemOverview> divisions,
            List<ItemOverview> managers,
            List<ItemOverview> groups);
    }
}