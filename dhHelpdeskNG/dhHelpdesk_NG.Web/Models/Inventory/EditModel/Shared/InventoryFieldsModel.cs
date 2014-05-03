namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using System;

    public class InventoryFieldsModel
    {
        public InventoryFieldsModel(ConfigurableFieldModel<string> barCode, ConfigurableFieldModel<DateTime?> purchaseDate)
        {
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
        }

        public ConfigurableFieldModel<string> BarCode { get; set; }

        public ConfigurableFieldModel<DateTime?> PurchaseDate { get; set; }
    }
}