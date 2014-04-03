namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
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
    }
}