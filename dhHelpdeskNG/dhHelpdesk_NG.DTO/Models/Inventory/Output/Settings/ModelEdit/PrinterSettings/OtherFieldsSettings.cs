namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(ModelEditFieldSetting numberOfTraysFieldSetting, ModelEditFieldSetting driverFieldSetting, ModelEditFieldSetting infoFieldSetting, ModelEditFieldSetting urlFieldSetting)
        {
            this.NumberOfTraysFieldSetting = numberOfTraysFieldSetting;
            this.DriverFieldSetting = driverFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting NumberOfTraysFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting DriverFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting InfoFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting URLFieldSetting { get; set; }
    }
}