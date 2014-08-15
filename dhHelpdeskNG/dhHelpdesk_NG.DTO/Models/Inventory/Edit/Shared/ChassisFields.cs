namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    public class ChassisFields
    {
        public ChassisFields(string chassis)
        {
            this.Chassis = chassis;
        }

        public string Chassis { get; set; }

        public static ChassisFields CreateDefault()
        {
            return new ChassisFields(null);
        }
    }
}