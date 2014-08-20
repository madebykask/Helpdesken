namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;
    using DH.Helpdesk.Web.Models.Inventory.OptionsAggregates;

    public interface IComputerViewModelBuilder
    {
        ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.ComputerForEdit model,
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings);

        ComputerViewModel BuildViewModel(
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}