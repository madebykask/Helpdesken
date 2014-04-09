namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsSettingsModel
    {
        public ChassisFieldsSettingsModel(FieldSettingModel chassisFieldSettingModel)
        {
            this.ChassisFieldSettingModel = chassisFieldSettingModel;
        }

        [IsId]
        public FieldSettingModel ChassisFieldSettingModel { get; set; }
    }
}