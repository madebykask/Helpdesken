namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SupplierFieldSettings
    {
        public SupplierFieldSettings(
                TextFieldSettings supplierOrderNumber, 
                TextFieldSettings supplierOrderDate, 
                TextFieldSettings supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        [NotNull]
        public TextFieldSettings SupplierOrderNumber { get; private set; }
         
        [NotNull]
        public TextFieldSettings SupplierOrderDate { get; private set; }
         
        [NotNull]
        public TextFieldSettings SupplierOrderInfo { get; private set; }                  
    }
}