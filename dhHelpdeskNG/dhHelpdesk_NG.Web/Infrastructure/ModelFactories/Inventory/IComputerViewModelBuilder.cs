namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;

    public interface IComputerViewModelBuilder
    {
        ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.Computer model,
            ComputerEditOptionsResponse options,
            ComputerFieldsSettingsForModelEdit settings);

        ComputerViewModel BuildViewModel(
            ComputerEditOptionsResponse options,
            ComputerFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}