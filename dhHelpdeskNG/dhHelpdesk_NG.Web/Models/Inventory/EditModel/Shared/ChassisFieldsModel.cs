namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
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