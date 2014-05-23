namespace DH.Helpdesk.Web.Models.Inventory
{
    using DH.Helpdesk.Web.Models.Inventory.EditModel;

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