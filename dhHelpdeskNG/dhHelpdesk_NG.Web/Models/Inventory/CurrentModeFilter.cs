namespace DH.Helpdesk.Web.Models.Inventory
{
    public class IndexModeFilter
    {
        public IndexModeFilter(int currentMode)
        {
            this.CurrentMode = currentMode;
        }

        public int CurrentMode { get; private set; }

        public static IndexModeFilter GetDefault()
        {
            return new IndexModeFilter((int)CurrentModes.Workstations);
        }
    }
}