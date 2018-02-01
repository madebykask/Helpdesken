namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface INewNotifierModelFactory
    {
        InputModel Create(
            NotifierOverviewSettings settings,
            List<ItemOverview> domains,
            List<ItemOverview> regions,
            List<ItemOverview> departments,
            List<ItemOverview> organizationUnits,
            List<ItemOverview> divisions,
            List<ItemOverview> managers,
            List<ItemOverview> groups,
            Dictionary<string, string> inputParams,
			List<ItemOverview> languages,
			ComputerUserCategoryModel model);
    }
}