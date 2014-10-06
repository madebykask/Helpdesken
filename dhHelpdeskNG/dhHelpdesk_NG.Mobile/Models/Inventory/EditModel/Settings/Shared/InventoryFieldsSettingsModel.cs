namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class InventoryFieldsSettingsModel
    {
        public InventoryFieldsSettingsModel()
        {
        }

        public InventoryFieldsSettingsModel(
            FieldSettingModel barCodeFieldSettingModel,
            FieldSettingModel purchaseDateFieldSettingModel)
        {
            this.BarCodeFieldSettingModel = barCodeFieldSettingModel;
            this.PurchaseDateFieldSettingModel = purchaseDateFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Bar Code")]
        public FieldSettingModel BarCodeFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Purchase Date")]
        public FieldSettingModel PurchaseDateFieldSettingModel { get; set; }
    }
}