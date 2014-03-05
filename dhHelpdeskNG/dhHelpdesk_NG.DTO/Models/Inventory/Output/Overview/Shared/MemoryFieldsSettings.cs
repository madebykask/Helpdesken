namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Shared
{
    public class MemoryFieldsSettings
    {
        public MemoryFieldsSettings(string ramName)
        {
            this.RAMName = ramName;
        }

        public string RAMName { get; set; }
    }
}