namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsModel
    {
        public ProccesorFieldsModel(int? proccesorId)
        {
            this.ProccesorId = proccesorId;
        }

        [IsId]
        public int? ProccesorId { get; set; }
    }
}