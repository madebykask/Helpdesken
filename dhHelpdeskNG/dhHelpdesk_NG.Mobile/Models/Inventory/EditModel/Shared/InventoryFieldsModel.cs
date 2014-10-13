namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Shared
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsModel
    {
        public InventoryFieldsModel()
        {
        }

        public InventoryFieldsModel(ConfigurableFieldModel<string> barCode, ConfigurableFieldModel<DateTime?> purchaseDate)
        {
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
        }

        [NotNull]
        public ConfigurableFieldModel<string> BarCode { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> PurchaseDate { get; set; }
    }
}