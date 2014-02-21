namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsSettings
    {
        public MemoryFieldsSettings(FieldSettingOverview ramFieldSetting)
        {
            this.RAMFieldSetting = ramFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview RAMFieldSetting { get; set; }
    }
}