namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(FieldSettingOverview infoFieldSetting, FieldSettingOverview otherFieldSetting, FieldSettingOverview urlFieldSetting, FieldSettingOverview url2FieldSetting, FieldSettingOverview ownerFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
            this.OtherFieldSetting = otherFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
            this.URL2FieldSetting = url2FieldSetting;
            this.OwnerFieldSetting = ownerFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview InfoFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview OtherFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview URLFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview URL2FieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview OwnerFieldSetting { get; set; }
    }
}