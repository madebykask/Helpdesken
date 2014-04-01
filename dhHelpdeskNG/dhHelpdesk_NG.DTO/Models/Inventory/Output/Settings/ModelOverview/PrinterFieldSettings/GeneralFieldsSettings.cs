namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(FieldSettingOverview nameFieldSetting, FieldSettingOverview manufacturerFieldSetting, FieldSettingOverview modelFieldSetting, FieldSettingOverview serialNumberFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NameFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ModelFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SerialNumberFieldSetting { get; set; }
    }
}