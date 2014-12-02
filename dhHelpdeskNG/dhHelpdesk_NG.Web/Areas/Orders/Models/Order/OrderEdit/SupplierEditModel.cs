namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class SupplierEditModel
    {
        public SupplierEditModel()
        {            
        }

        public SupplierEditModel(
            ConfigurableFieldModel<string> supplierOrderNumber,
            ConfigurableFieldModel<DateTime?> supplierOrderDate,
            ConfigurableFieldModel<string> supplierOrderInfo)
        {
            this.SupplierOrderNumber = supplierOrderNumber;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderInfo = supplierOrderInfo;
        }

        [NotNull]
        public ConfigurableFieldModel<string> SupplierOrderNumber { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<DateTime?> SupplierOrderDate { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> SupplierOrderInfo { get; set; } 
    }
}