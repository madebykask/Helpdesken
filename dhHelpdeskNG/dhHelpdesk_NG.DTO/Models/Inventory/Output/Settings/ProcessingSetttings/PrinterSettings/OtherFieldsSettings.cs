namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(
            ProcessingFieldSetting numberOfTraysFieldSetting,
            ProcessingFieldSetting driverFieldSetting,
            ProcessingFieldSetting infoFieldSetting,
            ProcessingFieldSetting urlFieldSetting)
        {
            this.NumberOfTraysFieldSetting = numberOfTraysFieldSetting;
            this.DriverFieldSetting = driverFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting NumberOfTraysFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting DriverFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting InfoFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting URLFieldSetting { get; set; }
    }
}