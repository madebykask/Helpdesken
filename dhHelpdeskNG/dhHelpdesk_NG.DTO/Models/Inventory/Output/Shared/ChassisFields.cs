namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class ChassisFields
    {
        public ChassisFields(string chassis)
        {
            this.Chassis = chassis;
        }

        public string Chassis { get; set; }
    }
}