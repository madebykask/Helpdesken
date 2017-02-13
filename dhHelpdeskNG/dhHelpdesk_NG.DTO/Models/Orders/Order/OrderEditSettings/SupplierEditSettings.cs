using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SupplierEditSettings : HeaderSettings
    {
        public SupplierEditSettings(
                TextFieldEditSettings supplierOrderNumber, 
                TextFieldEditSettings supplierOrderDate, 
                TextFieldEditSettings supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        [NotNull]
        public TextFieldEditSettings SupplierOrderNumber { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings SupplierOrderDate { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings SupplierOrderInfo { get; private set; } 
    }
}