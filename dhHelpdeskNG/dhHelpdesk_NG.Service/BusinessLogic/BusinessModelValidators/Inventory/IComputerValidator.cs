namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;

    using Computer = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer.Computer;

    public interface IComputerValidator
    {
        void Validate(Computer updatedComputer, Computer existingComputer, ComputerFieldsSettingsProcessing settings);

        void Validate(Computer newComputer, ComputerFieldsSettingsProcessing settings);
    }
}