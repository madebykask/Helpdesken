namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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