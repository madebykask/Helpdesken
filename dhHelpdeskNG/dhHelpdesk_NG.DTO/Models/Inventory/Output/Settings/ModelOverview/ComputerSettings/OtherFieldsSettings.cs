namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(FieldSettingOverview infoFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview InfoFieldSetting { get; set; }
    }
}