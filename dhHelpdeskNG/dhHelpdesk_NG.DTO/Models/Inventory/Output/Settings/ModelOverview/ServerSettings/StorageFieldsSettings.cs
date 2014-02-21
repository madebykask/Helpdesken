namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageFieldsSettings
    {
        public StorageFieldsSettings(FieldSettingOverview capasityFieldSetting)
        {
            this.CapasityFieldSetting = capasityFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview CapasityFieldSetting { get; set; }
    }
}