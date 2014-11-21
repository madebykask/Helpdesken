namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SupplierFieldSettings
    {
        public SupplierFieldSettings(
                FieldSettings supplierOrderNumber, 
                FieldSettings supplierOrderDate, 
                FieldSettings supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        [NotNull]
        public FieldSettings SupplierOrderNumber { get; private set; }
         
        [NotNull]
        public FieldSettings SupplierOrderDate { get; private set; }
         
        [NotNull]
        public FieldSettings SupplierOrderInfo { get; private set; }                  
    }
}