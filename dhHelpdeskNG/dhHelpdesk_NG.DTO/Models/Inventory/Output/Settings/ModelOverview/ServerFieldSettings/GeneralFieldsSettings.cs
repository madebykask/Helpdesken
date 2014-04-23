namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(
            FieldSettingOverview nameFieldSetting,
            FieldSettingOverview manufacturerFieldSetting,
            FieldSettingOverview descriptionFieldSetting,
            FieldSettingOverview modelFieldSetting,
            FieldSettingOverview serialNumberFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.DescriptionFieldSetting = descriptionFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NameFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview DescriptionFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ModelFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SerialNumberFieldSetting { get; set; }
    }
}