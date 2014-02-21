namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(FieldSetting numberOfTraysFieldSetting, FieldSetting driverFieldSetting, FieldSetting infoFieldSetting, FieldSetting urlFieldSetting)
        {
            this.NumberOfTraysFieldSetting = numberOfTraysFieldSetting;
            this.DriverFieldSetting = driverFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
        }

        [NotNull]
        public FieldSetting NumberOfTraysFieldSetting { get; set; }

        [NotNull]
        public FieldSetting DriverFieldSetting { get; set; }

        [NotNull]
        public FieldSetting InfoFieldSetting { get; set; }

        [NotNull]
        public FieldSetting URLFieldSetting { get; set; }
    }
}