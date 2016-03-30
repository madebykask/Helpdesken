namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
        [LocalizedDisplay("Streckkod")]
        public FieldSettingModel BarCodeFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Inköpsdatum")]
        public FieldSettingModel PurchaseDateFieldSettingModel { get; set; }
    }
}