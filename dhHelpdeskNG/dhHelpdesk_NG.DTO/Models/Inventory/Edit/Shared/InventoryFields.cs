namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using System;

    public class InventoryFields
    {
        public InventoryFields(string barCode, DateTime? purchaseDate)
        {
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
        }

        public string BarCode { get; set; }

        public DateTime? PurchaseDate { get; set; }
    }
}