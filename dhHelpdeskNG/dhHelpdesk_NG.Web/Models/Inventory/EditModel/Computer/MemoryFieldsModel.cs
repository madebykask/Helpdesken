namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsModel
    {
        public MemoryFieldsModel()
        {
        }

        public MemoryFieldsModel(int? ramId)
        {
            this.RAMId = ramId;
        }

        [IsId]
        public int? RAMId { get; set; }
    }
}