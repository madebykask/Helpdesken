namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class MemoryFields
    {
        public MemoryFields(string ramName)
        {
            this.RAMName = ramName;
        }

        public string RAMName { get; set; }
    }
}