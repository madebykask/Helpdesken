namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    public class ProccesorFields
    {
        public ProccesorFields(int? proccesorId)
        {
            this.ProccesorId = proccesorId;
        }

        public int? ProccesorId { get; set; }
    }
}