namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProcessorFieldsSettings
    {
        public ProcessorFieldsSettings(FieldSettingOverview proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public FieldSettingOverview ProccesorFieldSetting { get; set; }
    }
}