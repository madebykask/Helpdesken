namespace DH.Helpdesk.Domain.Orders
{
    using global::System;

    public class OrderHistoryEntity : Entity
    {
        public Guid OrderHistoryGuid { get; set; }

        public int OrderId { get; set; }

        public decimal? CaseNumber { get; set; }
        public int CreatedByUser_Id { get; set; }
        public int Deleted { get; set; }
        public int? Department_Id { get; set; }
        public int? Domain_Id { get; set; }
        public int OrderInfo2 { get; set; }
        public int? OrderState_Id { get; set; }
        public int OrderType_Id { get; set; }

        public int? DeliveryDepartmentId { get; set; }

        public string DeliveryOu { get; set; }

        public string DeliveryAddress { get; set; }

        public string DeliveryPostalCode { get; set; }

        public string DeliveryPostalAddress { get; set; }

        public string DeliveryLocation { get; set; }

        public int? OU_Id { get; set; }

        public int? OrderPropertyId { get; set; }

        public int? User_Id { get; set; }
        public string Configuration { get; set; }
        public string DeliveryInfo { get; set; }
        public string DeliveryInfo2 { get; set; }
        public string DeliveryInfo3 { get; set; }
        public string Filename { get; set; }
        public string Info { get; set; }
        public string MarkOfGoods { get; set; }
        public string Orderer { get; set; }
        public string OrdererAddress { get; set; }
        public string OrdererCode { get; set; }
        public string OrdererEMail { get; set; }
        public string OrdererID { get; set; }
        public string OrdererInvoiceAddress { get; set; }
        public string OrdererLocation { get; set; }
        public string OrdererPhone { get; set; }
        public string OrdererReferenceNumber { get; set; }

        public string AccountingDimension1 { get; set; }

        public string AccountingDimension2 { get; set; }

        public string AccountingDimension3 { get; set; }

        public string AccountingDimension4 { get; set; }

        public string AccountingDimension5 { get; set; }

        public string OrderInfo { get; set; }
        public string OrderRow { get; set; }
        public string OrderRow2 { get; set; }
        public string OrderRow3 { get; set; }
        public string OrderRow4 { get; set; }
        public string OrderRow5 { get; set; }
        public string OrderRow6 { get; set; }
        public string OrderRow7 { get; set; }
        public string OrderRow8 { get; set; }
        public string SupplierOrderInfo { get; set; }
        public string SupplierOrderNumber { get; set; }
        public string ReceiverEMail { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverLocation { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string UserFirstName { get; set; }
        public string UserId { get; set; }
        public string UserLastName { get; set; }
        public int? UserDepartment_Id { get; set; }
        public int? UserDepartment_Id2 { get; set; }
        public int? OrderFieldType_Id { get; set; }
//        public string OrderFieldType2 { get; set; }
        public int? OrderFieldType3_Id { get; set; }
        public int? OrderFieldType4_Id { get; set; }
        public int? OrderFieldType5_Id { get; set; }
        public int? UserOU_Id { get; set; }
        public int? EmploymentType_Id { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryPhone { get; set; }
        public string UserPhone { get; set; }
        public string UserEMail { get; set; }
        public string UserInitials { get; set; }
        public string UserPersonalIdentityNumber { get; set; }
        public string UserExtension { get; set; }
        public string UserTitle { get; set; }
        public string UserLocation { get; set; }
        public string UserRoomNumber { get; set; }
        public string UserPostalAddress { get; set; }
        public string Responsibility { get; set; }
        public string Activity { get; set; }
        public string Manager { get; set; }
        public string ReferenceNumber { get; set; }
        public string InfoUser { get; set; }
        public DateTime? AccountEndDate { get; set; }
        public DateTime? AccountStartDate { get; set; }
        public EMailTypes? EMailType { get; set; }
        public bool? HomeDirectory { get; set; }
        public bool? Profile { get; set; }
        public string InventoryNumber { get; set; }
        public string AccountInfo { get; set; }
        public string ContactEMail { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string InfoProduct { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Deliverydate { get; set; }
        public DateTime? InstallDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? SupplierOrderDate { get; set; }

        public virtual Domain Domain { get; set; }

        public virtual OU Ou { get; set; }

        public virtual OrderPropertyEntity OrderProperty { get; set; }

        public virtual OrderState OrderState { get; set; }

        public virtual OrderType OrderType { get; set; }

        public virtual Department DeliveryDepartment { get; set; }
     
        public virtual Order Order { get; set; }

        public virtual User Administrator { get; set; }

        public virtual Department Department { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual OrderFieldType OrderFieldType { get; set; }
        public virtual OrderFieldType OrderFieldType3 { get; set; }
        public virtual OrderFieldType OrderFieldType4 { get; set; }
        public virtual OrderFieldType OrderFieldType5 { get; set; }
        public virtual Department UserDepartment1 { get; set; }
        public virtual Department UserDepartment2 { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }
        public virtual OU UserOU { get; set; }
    }
}