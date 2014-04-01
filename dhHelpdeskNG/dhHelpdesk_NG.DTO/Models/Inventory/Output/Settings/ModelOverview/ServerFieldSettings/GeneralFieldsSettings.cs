namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(FieldSettingOverview serverNameFieldSetting, FieldSettingOverview manufacturerFieldSetting, FieldSettingOverview descriptionFieldSetting, FieldSettingOverview computerModelFieldSetting, FieldSettingOverview serialNumberFieldSetting)
        {
            this.ServerNameFieldSetting = serverNameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.DescriptionFieldSetting = descriptionFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview ServerNameFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview DescriptionFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ComputerModelFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SerialNumberFieldSetting { get; set; }
    }
}