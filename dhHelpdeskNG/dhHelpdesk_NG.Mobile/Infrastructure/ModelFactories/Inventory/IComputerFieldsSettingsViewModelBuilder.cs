namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer;

    public interface IComputerFieldsSettingsViewModelBuilder
    {
        ComputerFieldsSettingsViewModel BuildViewModel(ComputerFieldsSettings settings, List<ItemOverview> langauges, int langaugeId);
    }
}