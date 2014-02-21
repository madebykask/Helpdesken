namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSettingOverview createdDateFieldSetting, FieldSettingOverview changedDateFieldSetting, FieldSettingOverview syncChangeDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangeDateFieldSetting = syncChangeDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ChangedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SyncChangeDateFieldSetting { get; set; }
    }
}