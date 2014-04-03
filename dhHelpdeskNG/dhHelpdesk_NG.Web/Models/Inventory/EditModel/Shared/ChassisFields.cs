namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    public class ChassisFields
    {
        public ChassisFields(ConfigurableFieldModel<string> chassis)
        {
            this.Chassis = chassis;
        }

        public ConfigurableFieldModel<string> Chassis { get; set; }
    }
}