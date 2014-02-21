namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class MemoryFieldsSettings
    {
        public MemoryFieldsSettings(int? ramId)
        {
            this.RAMId = ramId;
        }

        public int? RAMId { get; set; }
    }
}