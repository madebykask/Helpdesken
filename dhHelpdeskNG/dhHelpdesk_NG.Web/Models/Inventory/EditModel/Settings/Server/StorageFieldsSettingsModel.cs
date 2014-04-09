namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageFieldsSettingsModel
    {
        public StorageFieldsSettingsModel(FieldSettingModel capasityFieldSettingModel)
        {
            this.CapasityFieldSettingModel = capasityFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel CapasityFieldSettingModel { get; set; }
    }
}