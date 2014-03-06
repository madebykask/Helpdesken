namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class ProccesorFieldsSettings
    {
        public ProccesorFieldsSettings(string proccesorName)
        {
            this.ProccesorName = proccesorName;
        }

        public string ProccesorName { get; set; }
    }
}