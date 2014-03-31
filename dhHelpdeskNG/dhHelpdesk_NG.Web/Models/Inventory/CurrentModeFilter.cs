namespace DH.Helpdesk.Web.Models.Inventory
{
    public class CurrentModeFilter
    {
        public CurrentModeFilter(CurrentModes currentMode)
        {
            this.CurrentMode = currentMode;
        }

        public CurrentModes CurrentMode { get; private set; }

        public static CurrentModeFilter GetDefaultFilter()
        {
            return new CurrentModeFilter(CurrentModes.Workstations);
        }
    }
}