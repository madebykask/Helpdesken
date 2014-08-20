namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;

    public interface IComputerValidator
    {
        void Validate(ComputerForUpdate updatedComputer, ComputerForEdit existingComputer, ComputerFieldsSettingsProcessing settings);

        void Validate(ComputerForInsert newComputer, ComputerFieldsSettingsProcessing settings);
    }
}