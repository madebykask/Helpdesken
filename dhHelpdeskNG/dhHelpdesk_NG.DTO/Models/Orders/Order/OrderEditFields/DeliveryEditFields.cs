namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;

    using Common.ValidationAttributes;

    public sealed class DeliveryEditFields
    {
        public DeliveryEditFields(
                DateTime? deliveryDate, 
                DateTime? installDate, 
                int? deliveryDepartmentId, 
                string deliveryOu, 
                string deliveryAddress, 
                string deliveryPostalCode, 
                string deliveryPostalAddress, 
                string deliveryLocation, 
                string deliveryInfo1, 
                string deliveryInfo2, 
                string deliveryInfo3,
                int? deliveryOuIdId,
                string deliveryName,
                string deliveryPhone)
        {
            DeliveryOuIdId = deliveryOuIdId;
            DeliveryInfo3 = deliveryInfo3;
            DeliveryInfo2 = deliveryInfo2;
            DeliveryInfo1 = deliveryInfo1;
            DeliveryLocation = deliveryLocation;
            DeliveryPostalAddress = deliveryPostalAddress;
            DeliveryPostalCode = deliveryPostalCode;
            DeliveryAddress = deliveryAddress;
            DeliveryOu = deliveryOu;
            DeliveryDepartmentId = deliveryDepartmentId;
            InstallDate = installDate;
            DeliveryDate = deliveryDate;
            DeliveryName = deliveryName;
            DeliveryPhone = deliveryPhone;
        }

        public DateTime? DeliveryDate { get; private set; }
        
        public DateTime? InstallDate { get; private set; }
        
        [IsId]
        public int? DeliveryDepartmentId { get; private set; }
        
        public string DeliveryOu { get; private set; }
        
        public string DeliveryAddress { get; private set; }
        
        public string DeliveryPostalCode { get; private set; }
        
        public string DeliveryPostalAddress { get; private set; }
        
        public string DeliveryLocation { get; private set; }
        
        [NotNull]
        public string DeliveryInfo1 { get; private set; }
        
        [NotNull]
        public string DeliveryInfo2 { get; private set; }
        
        [NotNull]
        public string DeliveryInfo3 { get; private set; }

        [IsId]
        public int? DeliveryOuIdId { get; private set; }

        public string DeliveryName { get; private set; }

        public string DeliveryPhone { get; private set; }
    }
}