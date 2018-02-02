namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface INotifierModelFactory
    {
		InputModel Create(
			NotifierOverviewSettings settings,
			int? selectedRegionId,
			NotifierDetails notifier,
			List<ItemOverview> domains,
			List<ItemOverview> regions,
			List<ItemOverview> departments,
			List<ItemOverview> organizationUnits,
			List<ItemOverview> divisions,
			List<ItemOverview> managers,
			List<ItemOverview> groups,
			List<ItemOverview> languages,
			ComputerUserCategoryModel categoryModel);
    }
}