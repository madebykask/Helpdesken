namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class ChassisFieldsSettings
    {
        public ChassisFieldsSettings(string chassis)
        {
            this.Chassis = chassis;
        }

        public string Chassis { get; set; }
    }
}