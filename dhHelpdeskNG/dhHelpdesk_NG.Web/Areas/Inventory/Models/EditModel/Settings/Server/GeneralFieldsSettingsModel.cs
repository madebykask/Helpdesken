namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class GeneralFieldsSettingsModel
    {
        public GeneralFieldsSettingsModel()
        {
        }

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
        [LocalizedDisplay("Namn")]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Tillverkare")]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Beskrivning")]
        public FieldSettingModel DescriptionFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Modell")]
        public FieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Serienummer")]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }
    }
}