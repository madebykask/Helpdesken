namespace DH.Helpdesk.Mobile.Models.Inventory
{
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel;

    public class ComputerModuleModeFilter
    {
        public ComputerModuleModeFilter(int moduleType)
        {
            this.ModuleType = moduleType;
        }

        public int ModuleType { get; private set; }

        public static ComputerModuleModeFilter GetDefault()
        {
            return new ComputerModuleModeFilter((int)ModuleTypes.Processor);
        }
    }
}