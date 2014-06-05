namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class OtherFieldsSettingsModel
    {
        public OtherFieldsSettingsModel()
        {
        }

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
        [LocalizedDisplay("Info")]
        public FieldSettingModel InfoFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Other")]
        public FieldSettingModel OtherFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Url")]
        public FieldSettingModel URLFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Url2")]
        public FieldSettingModel URL2FieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Owner")]
        public FieldSettingModel OwnerFieldSettingModel { get; set; }
    }
}