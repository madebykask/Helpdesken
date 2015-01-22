namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System;

    public sealed class DeliveryHistoryFields
    {
        public DeliveryHistoryFields(
                DateTime? deliveryDate, 
                DateTime? installDate, 
                int? deliveryDepartmentId,
                string deliveryDepartment, 
                string deliveryOu, 
                string deliveryAddress, 
                string deliveryPostalCode, 
                string deliveryPostalAddress, 
                string deliveryLocation, 
                string deliveryInfo1, 
                string deliveryInfo2, 
                string deliveryInfo3)
        {
            this.DeliveryDepartmentId = deliveryDepartmentId;
            this.DeliveryInfo3 = deliveryInfo3;
            this.DeliveryInfo2 = deliveryInfo2;
            this.DeliveryInfo1 = deliveryInfo1;
            this.DeliveryLocation = deliveryLocation;
            this.DeliveryPostalAddress = deliveryPostalAddress;
            this.DeliveryPostalCode = deliveryPostalCode;
            this.DeliveryAddress = deliveryAddress;
            this.DeliveryOu = deliveryOu;
            this.DeliveryDepartment = deliveryDepartment;
            this.InstallDate = installDate;
            this.DeliveryDate = deliveryDate;
        }

        public DateTime? DeliveryDate { get; private set; }
        
        public DateTime? InstallDate { get; private set; }
        
        public int? DeliveryDepartmentId { get; private set; }

        public string DeliveryDepartment { get; private set; }
        
        public string DeliveryOu { get; private set; }
        
        public string DeliveryAddress { get; private set; }
        
        public string DeliveryPostalCode { get; private set; }
        
        public string DeliveryPostalAddress { get; private set; }
        
        public string DeliveryLocation { get; private set; }
        
        public string DeliveryInfo1 { get; private set; }
        
        public string DeliveryInfo2 { get; private set; }
        
        public string DeliveryInfo3 { get; private set; }
    }
}