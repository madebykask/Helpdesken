namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class ProccesorFields
    {
        public ProccesorFields(string proccesorName)
        {
            this.ProccesorName = proccesorName;
        }

        public string ProccesorName { get; set; }
    }
}