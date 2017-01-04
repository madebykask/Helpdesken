namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    using System;

    public sealed class DeliveryOverview
    {
        public DeliveryOverview(
                DateTime? deliveryDate, 
                DateTime? installDate, 
                string deliveryDepartment, 
                string deliveryOu, 
                string deliveryAddress, 
                string deliveryPostalCode, 
                string deliveryPostalAddress, 
                string deliveryLocation, 
                string deliveryInfo1, 
                string deliveryInfo2, 
                string deliveryInfo3,
                string deliveryOuId,
                string deliveryName,
                string deliveryPhone)
        {
            DeliveryOuId = deliveryOuId;
            DeliveryInfo3 = deliveryInfo3;
            DeliveryInfo2 = deliveryInfo2;
            DeliveryInfo1 = deliveryInfo1;
            DeliveryLocation = deliveryLocation;
            DeliveryPostalAddress = deliveryPostalAddress;
            DeliveryPostalCode = deliveryPostalCode;
            DeliveryAddress = deliveryAddress;
            DeliveryOu = deliveryOu;
            DeliveryDepartment = deliveryDepartment;
            InstallDate = installDate;
            DeliveryDate = deliveryDate;
            DeliveryName = deliveryName;
            DeliveryPhone = deliveryPhone;
        }

        public DateTime? DeliveryDate { get; private set; }
        
        public DateTime? InstallDate { get; private set; }
        
        public string DeliveryDepartment { get; private set; }
        
        public string DeliveryOu { get; private set; }
        
        public string DeliveryAddress { get; private set; }
        
        public string DeliveryPostalCode { get; private set; }
        
        public string DeliveryPostalAddress { get; private set; }
        
        public string DeliveryLocation { get; private set; }
        
        public string DeliveryInfo1 { get; private set; }
        
        public string DeliveryInfo2 { get; private set; }
        
        public string DeliveryInfo3 { get; private set; }

        public string DeliveryOuId { get; private set; }

        public string DeliveryName { get; private set; }

        public string DeliveryPhone { get; private set; }
    }
}