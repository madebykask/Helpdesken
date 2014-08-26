namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings
{
    public class InventoryFieldSettingForProcessing
    {
        public InventoryFieldSettingForProcessing(
            bool showInDetails)
        {
            this.ShowInDetails = showInDetails;
        }

        public bool ShowInDetails { get; private set; }
    }
}