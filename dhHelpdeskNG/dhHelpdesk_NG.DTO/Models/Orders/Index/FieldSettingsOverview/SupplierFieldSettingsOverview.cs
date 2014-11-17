namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SupplierFieldSettingsOverview
    {
        public SupplierFieldSettingsOverview(
                FieldOverviewSetting supplierOrderNumber, 
                FieldOverviewSetting supplierOrderDate, 
                FieldOverviewSetting supplierOrderInfo)
        {
            this.SupplierOrderInfo = supplierOrderInfo;
            this.SupplierOrderDate = supplierOrderDate;
            this.SupplierOrderNumber = supplierOrderNumber;
        }

        [NotNull]
        public FieldOverviewSetting SupplierOrderNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting SupplierOrderDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting SupplierOrderInfo { get; private set; }         
    }
}