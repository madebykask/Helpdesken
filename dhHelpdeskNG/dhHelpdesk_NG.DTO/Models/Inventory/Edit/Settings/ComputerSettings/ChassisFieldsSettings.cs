namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
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