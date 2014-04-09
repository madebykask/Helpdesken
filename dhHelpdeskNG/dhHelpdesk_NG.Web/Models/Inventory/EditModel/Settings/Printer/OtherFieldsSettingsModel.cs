namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettingsModel
    {
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
        public FieldSettingModel NumberOfTraysFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel DriverFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel InfoFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel URLFieldSettingModel { get; set; }
    }
}