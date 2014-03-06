namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(FieldSetting infoFieldSetting, FieldSetting otherFieldSetting, FieldSetting urlFieldSetting, FieldSetting url2FieldSetting, FieldSetting ownerFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
            this.OtherFieldSetting = otherFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
            this.URL2FieldSetting = url2FieldSetting;
            this.OwnerFieldSetting = ownerFieldSetting;
        }

        [NotNull]
        public FieldSetting InfoFieldSetting { get; set; }

        [NotNull]
        public FieldSetting OtherFieldSetting { get; set; }

        [NotNull]
        public FieldSetting URLFieldSetting { get; set; }

        [NotNull]
        public FieldSetting URL2FieldSetting { get; set; }

        [NotNull]
        public FieldSetting OwnerFieldSetting { get; set; }
    }
}