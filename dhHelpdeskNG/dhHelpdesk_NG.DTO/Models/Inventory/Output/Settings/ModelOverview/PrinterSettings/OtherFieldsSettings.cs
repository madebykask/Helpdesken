namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(FieldSettingOverview numberOfTraysFieldSetting, FieldSettingOverview driverFieldSetting, FieldSettingOverview infoFieldSetting, FieldSettingOverview urlFieldSetting)
        {
            this.NumberOfTraysFieldSetting = numberOfTraysFieldSetting;
            this.DriverFieldSetting = driverFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NumberOfTraysFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview DriverFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview InfoFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview URLFieldSetting { get; set; }
    }
}