namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsSettings
    {
        public ChassisFieldsSettings(FieldSetting chassisFieldSetting)
        {
            this.ChassisFieldSetting = chassisFieldSetting;
        }

        [NotNull]
        public FieldSetting ChassisFieldSetting { get; set; }
    }
}