namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit;
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