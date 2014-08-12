namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsSettings
    {
        public InventoryFieldsSettings(ProcessingFieldSetting barCodeFieldSetting, ProcessingFieldSetting purchaseDateFieldSetting)
        {
            this.BarCodeFieldSetting = barCodeFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting BarCodeFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PurchaseDateFieldSetting { get; set; }
    }
}