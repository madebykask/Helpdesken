namespace DH.Helpdesk.Web.Models.Inventory
{
    public class CurrentModeFilter
    {
        public CurrentModeFilter(int currentMode)
        {
            this.CurrentMode = currentMode;
        }

        public int CurrentMode { get; private set; }

        public static CurrentModeFilter GetDefault()
        {
            return new CurrentModeFilter((int)CurrentModes.Workstations);
        }
    }
}