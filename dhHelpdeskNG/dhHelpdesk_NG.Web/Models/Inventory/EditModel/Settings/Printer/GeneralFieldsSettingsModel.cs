namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class GeneralFieldsSettingsModel
    {
        public GeneralFieldsSettingsModel(
            FieldSettingModel nameFieldSettingModel,
            FieldSettingModel manufacturerFieldSettingModel,
            FieldSettingModel modelFieldSettingModel,
            FieldSettingModel serialNumberFieldSettingModel)
        {
            this.NameFieldSettingModel = nameFieldSettingModel;
            this.ManufacturerFieldSettingModel = manufacturerFieldSettingModel;
            this.ModelFieldSettingModel = modelFieldSettingModel;
            this.SerialNumberFieldSettingModel = serialNumberFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Name")]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Manufacturer")]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Model")]
        public FieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Serial Number")]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }
    }
}