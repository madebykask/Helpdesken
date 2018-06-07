namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSettingOverview createdDateFieldSetting, 
                                   FieldSettingOverview changedDateFieldSetting,
                                   FieldSettingOverview syncChangedDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangedDateFieldSetting = syncChangedDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ChangedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SyncChangedDateFieldSetting { get; set; }
    }
}