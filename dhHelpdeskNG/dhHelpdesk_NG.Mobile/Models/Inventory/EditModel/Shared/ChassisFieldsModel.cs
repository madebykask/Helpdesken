namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Shared
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