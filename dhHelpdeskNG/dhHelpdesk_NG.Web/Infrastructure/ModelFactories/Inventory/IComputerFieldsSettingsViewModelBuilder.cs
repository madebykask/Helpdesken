namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer;

    public interface IComputerFieldsSettingsViewModelBuilder
    {
        ComputerFieldsSettingsViewModel BuildViewModel(ComputerFieldsSettings settings, WorkstationTabsSettings tabsSettings, List<ItemOverview> langauges, int langaugeId);
    }
}