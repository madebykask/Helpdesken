namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class ProcessorFields
    {
        public ProcessorFields(string proccesorName)
        {
            this.ProccesorName = proccesorName;
        }

        public string ProccesorName { get; set; }
    }
}