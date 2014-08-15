namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFields
    {
        public MemoryFields(int? ramId)
        {
            this.RAMId = ramId;
        }

        [IsId]
        public int? RAMId { get; set; }

        public static MemoryFields CreateDefault()
        {
            return new MemoryFields(null);
        }
    }
}