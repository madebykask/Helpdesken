namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsSettings
    {
        public InventoryFieldsSettings(FieldSetting barCodeFieldSetting, FieldSetting purchaseDateFieldSetting)
        {
            this.BarCodeFieldSetting = barCodeFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
        }

        [NotNull]
        public FieldSetting BarCodeFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PurchaseDateFieldSetting { get; set; }
    }
}