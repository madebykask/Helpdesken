namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsModel
    {
        public ChassisFieldsModel()
        {
        }

        public ChassisFieldsModel(ConfigurableFieldModel<string> chassis)
        {
            this.Chassis = chassis;
        }

        [NotNull]
        public ConfigurableFieldModel<string> Chassis { get; set; }
    }
}