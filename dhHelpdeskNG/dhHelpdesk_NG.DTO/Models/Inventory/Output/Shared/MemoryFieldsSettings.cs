namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
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