namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    using DH.Helpdesk.Web.Enums.Inventory;

    public class InventoriesModeFilter
    {
        public InventoriesModeFilter(int currentMode)
        {
            this.CurrentMode = currentMode;
        }

        public int CurrentMode { get; private set; }

        public static InventoriesModeFilter GetDefault()
        {
            return new InventoriesModeFilter((int)CurrentModes.Workstations);
        }
    }
}