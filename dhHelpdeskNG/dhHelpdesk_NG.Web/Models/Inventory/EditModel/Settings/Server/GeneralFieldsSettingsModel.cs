namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettingsModel
    {
        public GeneralFieldsSettingsModel(
            FieldSettingModel nameFieldSettingModel,
            FieldSettingModel manufacturerFieldSettingModel,
            FieldSettingModel descriptionFieldSettingModel,
            FieldSettingModel modelFieldSettingModel,
            FieldSettingModel serialNumberFieldSettingModel)
        {
            this.NameFieldSettingModel = nameFieldSettingModel;
            this.ManufacturerFieldSettingModel = manufacturerFieldSettingModel;
            this.DescriptionFieldSettingModel = descriptionFieldSettingModel;
            this.ModelFieldSettingModel = modelFieldSettingModel;
            this.SerialNumberFieldSettingModel = serialNumberFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel DescriptionFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }
    }
}