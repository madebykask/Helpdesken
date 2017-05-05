using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class SupplierFieldSettingsModel
    {
        public SupplierFieldSettingsModel()
        {            
        }

        public SupplierFieldSettingsModel(
                TextFieldSettingsModel supplierOrderNumber, 
                TextFieldSettingsModel supplierOrderDate, 
                TextFieldSettingsModel supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.SupplierOrderNumber)]
        public TextFieldSettingsModel SupplierOrderNumber { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.SupplierOrderDate)]
        public TextFieldSettingsModel SupplierOrderDate { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.SupplierOrderInfo)]
        public TextFieldSettingsModel SupplierOrderInfo { get; set; }  
    }
}