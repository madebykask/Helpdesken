namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SupplierEditFields
    {
        public SupplierEditFields(
                string supplierOrderNumber, 
                DateTime? supplierOrderDate, 
                string supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        [NotNull]
        public string SupplierOrderNumber { get; private set; }
        
        public DateTime? SupplierOrderDate { get; private set; }

        [NotNull]
        public string SupplierOrderInfo { get; private set; }   
    }
}