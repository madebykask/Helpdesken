namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProcessorFields
    {
        public ProcessorFields(int? proccesorId)
        {
            this.ProccesorId = proccesorId;
        }

        [IsId]
        public int? ProccesorId { get; set; }
    }
}