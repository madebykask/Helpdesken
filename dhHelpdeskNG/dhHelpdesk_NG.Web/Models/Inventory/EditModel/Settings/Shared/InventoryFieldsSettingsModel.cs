namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsSettingsModel
    {
        public InventoryFieldsSettingsModel(FieldSettingModel barCodeFieldSettingModel, FieldSettingModel purchaseDateFieldSettingModel)
        {
            this.BarCodeFieldSettingModel = barCodeFieldSettingModel;
            this.PurchaseDateFieldSettingModel = purchaseDateFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel BarCodeFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PurchaseDateFieldSettingModel { get; set; }
    }
}