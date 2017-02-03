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

        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay("Ordernummer")]
        public TextFieldSettingsModel SupplierOrderNumber { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Beställningsdatum")]
        public TextFieldSettingsModel SupplierOrderDate { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Info")]
        public TextFieldSettingsModel SupplierOrderInfo { get; set; }  
    }
}