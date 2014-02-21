namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsSettings
    {
        public ChassisFieldsSettings(FieldSetting chassisFieldSetting)
        {
            this.ChassisFieldSetting = chassisFieldSetting;
        }

        [IsId]
        public FieldSetting ChassisFieldSetting { get; set; }
    }
}