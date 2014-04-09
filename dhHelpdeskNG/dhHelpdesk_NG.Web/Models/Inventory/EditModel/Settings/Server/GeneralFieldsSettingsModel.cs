namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettingsModel
    {
        public GeneralFieldsSettingsModel(
            FieldSettingModel serverNameFieldSettingModel,
            FieldSettingModel manufacturerFieldSettingModel,
            FieldSettingModel descriptionFieldSettingModel,
            FieldSettingModel computerModelFieldSettingModel,
            FieldSettingModel serialNumberFieldSettingModel)
        {
            this.ServerNameFieldSettingModel = serverNameFieldSettingModel;
            this.ManufacturerFieldSettingModel = manufacturerFieldSettingModel;
            this.DescriptionFieldSettingModel = descriptionFieldSettingModel;
            this.ComputerModelFieldSettingModel = computerModelFieldSettingModel;
            this.SerialNumberFieldSettingModel = serialNumberFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel ServerNameFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel DescriptionFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ComputerModelFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }
    }
}