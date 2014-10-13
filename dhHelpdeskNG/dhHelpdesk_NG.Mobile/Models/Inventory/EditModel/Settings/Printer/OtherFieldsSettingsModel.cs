namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

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
        [LocalizedDisplay("Number Of Trays")]
        public FieldSettingModel NumberOfTraysFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Driver")]
        public FieldSettingModel DriverFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Info")]
        public FieldSettingModel InfoFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Url")]
        public FieldSettingModel URLFieldSettingModel { get; set; }
    }
}