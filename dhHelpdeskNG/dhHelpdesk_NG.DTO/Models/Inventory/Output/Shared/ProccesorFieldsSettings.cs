namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class ProccesorFieldsSettings
    {
        public ProccesorFieldsSettings(int? proccesorId)
        {
            this.ProccesorId = proccesorId;
        }

        public int? ProccesorId { get; set; }
    }
}