namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFieldsSettingsOverviewForShortInfo
    {
        public ComputerFieldsSettingsOverviewForShortInfo(
            FieldSettingOverview nameFieldSetting,
            FieldSettingOverview manufacturerFieldSetting,
            FieldSettingOverview computerModelFieldSetting,
            FieldSettingOverview serialNumberFieldSetting,
            FieldSettingOverview biosVersionFieldSetting,
            FieldSettingOverview biosDateFieldSetting,
            FieldSettingOverview operatingSystemFieldSetting,
            FieldSettingOverview servicePackFieldSetting,
            FieldSettingOverview processorFieldSetting,
            FieldSettingOverview memoryFieldSetting,
            FieldSettingOverview nicFieldSetting,
            FieldSettingOverview ipAddressFieldSetting,
            FieldSettingOverview macAddressFieldSetting,
            FieldSettingOverview rasFieldSetting,
            FieldSettingOverview infoFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
            this.BiosVersionFieldSetting = biosVersionFieldSetting;
            this.BiosDateFieldSetting = biosDateFieldSetting;
            this.OperatingSystemFieldSetting = operatingSystemFieldSetting;
            this.ServicePackFieldSetting = servicePackFieldSetting;
            this.ProcessorFieldSetting = processorFieldSetting;
            this.MemoryFieldSetting = memoryFieldSetting;
            this.NicFieldSetting = nicFieldSetting;
            this.IpAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
            this.RasFieldSetting = rasFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NameFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ManufacturerFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ComputerModelFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview SerialNumberFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview BiosVersionFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview BiosDateFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview OperatingSystemFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ServicePackFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ProcessorFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview MemoryFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview NicFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview IpAddressFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview MacAddressFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview RasFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview InfoFieldSetting { get; private set; }
    }
}