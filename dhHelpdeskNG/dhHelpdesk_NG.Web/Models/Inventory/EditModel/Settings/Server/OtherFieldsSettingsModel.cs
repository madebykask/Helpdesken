namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettingsModel
    {
        public OtherFieldsSettingsModel(
            FieldSettingModel infoFieldSettingModel,
            FieldSettingModel otherFieldSettingModel,
            FieldSettingModel urlFieldSettingModel,
            FieldSettingModel url2FieldSettingModel,
            FieldSettingModel ownerFieldSettingModel)
        {
            this.InfoFieldSettingModel = infoFieldSettingModel;
            this.OtherFieldSettingModel = otherFieldSettingModel;
            this.URLFieldSettingModel = urlFieldSettingModel;
            this.URL2FieldSettingModel = url2FieldSettingModel;
            this.OwnerFieldSettingModel = ownerFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel InfoFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel OtherFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel URLFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel URL2FieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel OwnerFieldSettingModel { get; set; }
    }
}