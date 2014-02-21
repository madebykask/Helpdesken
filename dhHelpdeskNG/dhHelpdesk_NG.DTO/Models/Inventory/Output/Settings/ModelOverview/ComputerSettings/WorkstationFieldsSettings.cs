namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsSettings
    {
        public WorkstationFieldsSettings(FieldSettingOverview computerNameFieldSetting, FieldSettingOverview manufacturerFieldSetting, FieldSettingOverview computerModelFieldSetting, FieldSettingOverview serialNumberFieldSetting, FieldSettingOverview biosVersionFieldSetting, FieldSettingOverview biosDateFieldSetting, FieldSettingOverview theftmarkFieldSetting, FieldSettingOverview carePackNumberFieldSetting, FieldSettingOverview computerTypeFieldSetting, FieldSettingOverview locationFieldSetting)
        {
            this.ComputerNameFieldSetting = computerNameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
            this.BIOSVersionFieldSetting = biosVersionFieldSetting;
            this.BIOSDateFieldSetting = biosDateFieldSetting;
            this.TheftmarkFieldSetting = theftmarkFieldSetting;
            this.CarePackNumberFieldSetting = carePackNumberFieldSetting;
            this.ComputerTypeFieldSetting = computerTypeFieldSetting;
            this.LocationFieldSetting = locationFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview ComputerNameFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ComputerModelFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SerialNumberFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview BIOSVersionFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview BIOSDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview TheftmarkFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview CarePackNumberFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ComputerTypeFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview LocationFieldSetting { get; set; }
    }
}