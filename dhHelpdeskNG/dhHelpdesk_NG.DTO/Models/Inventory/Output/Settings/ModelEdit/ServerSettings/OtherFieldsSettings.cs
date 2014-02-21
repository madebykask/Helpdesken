namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(ModelEditFieldSetting infoFieldSetting, ModelEditFieldSetting otherFieldSetting, ModelEditFieldSetting urlFieldSetting, ModelEditFieldSetting url2FieldSetting, ModelEditFieldSetting ownerFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
            this.OtherFieldSetting = otherFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
            this.URL2FieldSetting = url2FieldSetting;
            this.OwnerFieldSetting = ownerFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting InfoFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting OtherFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting URLFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting URL2FieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting OwnerFieldSetting { get; set; }
    }
}