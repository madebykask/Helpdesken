namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;

    public interface IComputerViewModelBuilder
    {
        ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.ComputerForRead model,
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings);

        ComputerViewModel BuildViewModel(
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}