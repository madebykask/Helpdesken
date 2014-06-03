namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class ChassisFieldsModel
    {
        public ChassisFieldsModel(ConfigurableFieldModel<string> chassis)
        {
            this.Chassis = chassis;
        }

        public ConfigurableFieldModel<string> Chassis { get; set; }
    }
}