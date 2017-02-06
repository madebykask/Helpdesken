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

        public string Header { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> SupplierOrderNumber { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<DateTime?> SupplierOrderDate { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> SupplierOrderInfo { get; set; }

        public static SupplierEditModel CreateEmpty()
        {
            return new SupplierEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return this.SupplierOrderNumber.Show ||
                this.SupplierOrderDate.Show ||
                this.SupplierOrderInfo.Show;
        }
    }
}