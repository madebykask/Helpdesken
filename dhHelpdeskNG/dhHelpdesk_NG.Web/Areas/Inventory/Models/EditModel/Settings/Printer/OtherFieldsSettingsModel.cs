namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class OtherFieldsSettingsModel
    {
        public OtherFieldsSettingsModel()
        {
        }

        public OtherFieldsSettingsModel(
            FieldSettingModel numberOfTraysFieldSettingModel,
            FieldSettingModel driverFieldSettingModel,
            FieldSettingModel infoFieldSettingModel,
            FieldSettingModel urlFieldSettingModel)
        {
            this.NumberOfTraysFieldSettingModel = numberOfTraysFieldSettingModel;
            this.DriverFieldSettingModel = driverFieldSettingModel;
            this.InfoFieldSettingModel = infoFieldSettingModel;
            this.URLFieldSettingModel = urlFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Antal fack")]
        public FieldSettingModel NumberOfTraysFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Drivrutin")]
        public FieldSettingModel DriverFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Info")]
        public FieldSettingModel InfoFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Url")]
        public FieldSettingModel URLFieldSettingModel { get; set; }
    }
}