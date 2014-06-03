namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsSettings
    {
        public InventoryFieldsSettings(ModelEditFieldSetting barCodeFieldSetting, ModelEditFieldSetting purchaseDateFieldSetting)
        {
            this.BarCodeFieldSetting = barCodeFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting BarCodeFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PurchaseDateFieldSetting { get; set; }
    }
}