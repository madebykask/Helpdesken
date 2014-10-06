namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class ChassisFieldsSettingsModel
    {
        public ChassisFieldsSettingsModel()
        {
        }

        public ChassisFieldsSettingsModel(FieldSettingModel chassisFieldSettingModel)
        {
            this.ChassisFieldSettingModel = chassisFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Chassis")]
        public FieldSettingModel ChassisFieldSettingModel { get; set; }
    }
}