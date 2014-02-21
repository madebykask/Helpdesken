namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSettingOverview createdDateFieldSetting, FieldSettingOverview changedDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ChangedDateFieldSetting { get; set; }
    }
}