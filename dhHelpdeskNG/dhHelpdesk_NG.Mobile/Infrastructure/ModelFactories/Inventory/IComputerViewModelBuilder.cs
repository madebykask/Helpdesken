namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer;
    using DH.Helpdesk.Mobile.Models.Inventory.OptionsAggregates;

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