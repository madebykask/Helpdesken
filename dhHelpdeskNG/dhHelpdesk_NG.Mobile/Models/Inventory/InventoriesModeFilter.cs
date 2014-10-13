namespace DH.Helpdesk.Mobile.Models.Inventory
{
    using DH.Helpdesk.Mobile.Enums.Inventory;

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