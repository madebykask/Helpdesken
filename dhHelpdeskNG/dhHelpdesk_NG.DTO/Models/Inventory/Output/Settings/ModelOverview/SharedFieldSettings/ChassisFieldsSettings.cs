namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsSettings
    {
        public ChassisFieldsSettings(FieldSettingOverview chassisFieldSetting)
        {
            this.ChassisFieldSetting = chassisFieldSetting;
        }

        [IsId]
        public FieldSettingOverview ChassisFieldSetting { get; set; }
    }
}