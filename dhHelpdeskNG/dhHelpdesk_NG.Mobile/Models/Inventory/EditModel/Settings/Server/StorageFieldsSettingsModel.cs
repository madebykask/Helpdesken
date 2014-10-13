namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class StorageFieldsSettingsModel
    {
        public StorageFieldsSettingsModel()
        {
        }

        public StorageFieldsSettingsModel(FieldSettingModel capasityFieldSettingModel)
        {
            this.CapasityFieldSettingModel = capasityFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Capasity")]
        public FieldSettingModel CapasityFieldSettingModel { get; set; }
    }
}