namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;

    public interface IComputerRestorer
    {
        void Restore(ComputerForUpdate computer, ComputerForRead existingComputer, ComputerFieldsSettingsProcessing settings);
    }
}