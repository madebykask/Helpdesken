namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsSettings
    {
        public InventoryFieldsSettings(FieldSettingOverview barCodeFieldSetting, FieldSettingOverview purchaseDateFieldSetting)
        {
            this.BarCodeFieldSetting = barCodeFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview BarCodeFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PurchaseDateFieldSetting { get; set; }
    }
}