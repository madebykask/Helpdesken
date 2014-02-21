namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(FieldSetting infoFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
        }

        [NotNull]
        public FieldSetting InfoFieldSetting { get; set; }
    }
}