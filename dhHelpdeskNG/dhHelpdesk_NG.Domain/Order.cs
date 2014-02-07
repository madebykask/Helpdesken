namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Order : Entity
    {
        public decimal CaseNumber { get; set; }
        public int ChangedByUser_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Deleted { get; set; }
        public int Department_Id { get; set; }
        public int Domain_Id { get; set; }
        public int OrderInfo2 { get; set; }
        public int OrderState_Id { get; set; }
        public int OrderType_Id { get; set; }
        public int OU_Id { get; set; }
        public int User_Id { get; set; }
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
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deliverydate { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime SupplierOrderDate { get; set; }
    }
}
