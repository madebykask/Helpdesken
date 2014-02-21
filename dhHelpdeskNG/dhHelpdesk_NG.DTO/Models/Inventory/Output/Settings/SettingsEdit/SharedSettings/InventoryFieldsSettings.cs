namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
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