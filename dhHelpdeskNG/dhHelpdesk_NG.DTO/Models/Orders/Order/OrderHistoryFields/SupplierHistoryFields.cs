namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System;

    public sealed class SupplierHistoryFields
    {
        public SupplierHistoryFields(
                string supplierOrderNumber, 
                DateTime? supplierOrderDate, 
                string supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        public string SupplierOrderNumber { get; private set; }
        
        public DateTime? SupplierOrderDate { get; private set; }

        public string SupplierOrderInfo { get; private set; }   
    }
}