namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsSettings
    {
        public ProccesorFieldsSettings(FieldSettingOverview proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public FieldSettingOverview ProccesorFieldSetting { get; set; }
    }
}