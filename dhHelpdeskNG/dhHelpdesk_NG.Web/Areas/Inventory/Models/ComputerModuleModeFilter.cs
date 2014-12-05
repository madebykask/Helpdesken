namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;

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