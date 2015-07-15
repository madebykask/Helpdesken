namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
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
        [LocalizedDisplay("Chassi")]
        public FieldSettingModel ChassisFieldSettingModel { get; set; }
    }
}