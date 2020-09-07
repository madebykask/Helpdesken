namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class DocumentFieldsSettingsModel
    {
        public DocumentFieldsSettingsModel()
        {
        }

        public DocumentFieldsSettingsModel(FieldSettingModel documentFieldSettingModel)
        {
            this.DocumentFieldSettingModel = documentFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Bifogad fil")]
        public FieldSettingModel DocumentFieldSettingModel { get; set; }
    }
}