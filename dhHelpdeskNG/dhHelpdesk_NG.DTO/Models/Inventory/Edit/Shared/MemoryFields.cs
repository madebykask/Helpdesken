namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    public class MemoryFields
    {
        public MemoryFields(int? ramId)
        {
            this.RAMId = ramId;
        }

        public int? RAMId { get; set; }
    }
}