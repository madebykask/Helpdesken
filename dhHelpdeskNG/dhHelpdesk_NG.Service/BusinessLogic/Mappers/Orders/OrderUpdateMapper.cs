namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Domain;

    public static class OrderUpdateMapper
    {
        public static void MapToEntity(Order entity, FullOrderEditFields businessModel, int customerId)
        {
            entity.Customer_Id = customerId;
            entity.OrderType_Id = businessModel.OrderTypeId;

            MapDeliveryFields(entity, businessModel.Delivery);
            MapGeneralFields(entity, businessModel.General);
            MapLogFields(entity, businessModel.Log);
            MapOrdererFields(entity, businessModel.Orderer);
            MapOrderFields(entity, businessModel.Order);
            MapOtherFields(entity, businessModel.Other);
            MapProgramFields(entity, businessModel.Program);
            MapReceiverFields(entity, businessModel.Receiver);
            MapSupplierFields(entity, businessModel.Supplier);
            MapUserFields(entity, businessModel.User);
        }

        private static void MapDeliveryFields(Order entity, DeliveryEditFields businessModel)
        {
            entity.Deliverydate = businessModel.DeliveryDate;
            entity.InstallDate = businessModel.InstallDate;
            entity.DeliveryDepartmentId = businessModel.DeliveryDepartmentId;
            entity.DeliveryOu = businessModel.DeliveryOu;
            entity.DeliveryAddress = businessModel.DeliveryAddress;
            entity.DeliveryPostalCode = businessModel.DeliveryPostalCode;
            entity.DeliveryPostalAddress = businessModel.DeliveryPostalAddress;
            entity.DeliveryLocation = businessModel.DeliveryLocation;
            entity.DeliveryInfo = businessModel.DeliveryInfo1;
            entity.DeliveryInfo2 = businessModel.DeliveryInfo2;
            entity.DeliveryInfo3 = businessModel.DeliveryInfo3;
            entity.DeliveryOuId = businessModel.DeliveryOuIdId;
        } 

        private static void MapGeneralFields(Order entity, GeneralEditFields businessModel)
        {
            entity.User_Id = businessModel.AdministratorId;
            entity.Domain_Id = businessModel.DomainId;
            entity.OrderDate = businessModel.OrderDate;
            entity.OrderState_Id = businessModel.StatusId;
        } 

        private static void MapLogFields(Order entity, LogEditFields businessModel)
        {
            if (businessModel.Logs.Any())
            {
                entity.Logs = businessModel.Logs.Select(l => new OrderLog
                                                                 {
                                                                     Id = l.Id,
                                                                     LogNote = l.Text
                                                                 }).ToList();
            }
        } 

        private static void MapOrdererFields(Order entity, OrdererEditFields businessModel)
        {
            entity.OrdererID = businessModel.OrdererId;
            entity.Orderer = businessModel.OrdererName;
            entity.OrdererLocation = businessModel.OrdererLocation;
            entity.OrdererEMail = businessModel.OrdererEmail;
            entity.OrdererPhone = businessModel.OrdererPhone;
            entity.OrdererCode = businessModel.OrdererCode;
            entity.Department_Id = businessModel.DepartmentId;
            entity.OU_Id = businessModel.UnitId;
            entity.OrdererAddress = businessModel.OrdererAddress;
            entity.OrdererInvoiceAddress = businessModel.OrdererInvoiceAddress;
            entity.OrdererReferenceNumber = businessModel.OrdererReferenceNumber;
            entity.AccountingDimension1 = businessModel.AccountingDimension1;
            entity.AccountingDimension2 = businessModel.AccountingDimension2;
            entity.AccountingDimension3 = businessModel.AccountingDimension3;
            entity.AccountingDimension4 = businessModel.AccountingDimension4;
            entity.AccountingDimension5 = businessModel.AccountingDimension5;
        } 

        private static void MapOrderFields(Order entity, OrderEditFields businessModel)
        {
            entity.OrderPropertyId = businessModel.PropertyId;
            entity.OrderRow = businessModel.OrderRow1;
            entity.OrderRow2 = businessModel.OrderRow2;
            entity.OrderRow3 = businessModel.OrderRow3;
            entity.OrderRow4 = businessModel.OrderRow4;
            entity.OrderRow5 = businessModel.OrderRow5;
            entity.OrderRow6 = businessModel.OrderRow6;
            entity.OrderRow7 = businessModel.OrderRow7;
            entity.OrderRow8 = businessModel.OrderRow8;
            entity.Configuration = businessModel.Configuration;
            entity.OrderInfo = businessModel.OrderInfo;
            entity.OrderInfo2 = businessModel.OrderInfo2;
        } 

        private static void MapOtherFields(Order entity, OtherEditFields businessModel)
        {
            entity.Filename = businessModel.FileName;
            entity.CaseNumber = businessModel.CaseNumber;
            entity.Info = businessModel.Info;
        } 

        private static void MapProgramFields(Order entity, ProgramEditFields businessModel)
        {
            entity.Programs = businessModel.Programs.Select(p => new Program
                                                                     {
                                                                         Id = p.Id
                                                                     }).ToList();
        } 

        private static void MapReceiverFields(Order entity, ReceiverEditFields businessModel)
        {
            entity.ReceiverId = businessModel.ReceiverId;
            entity.ReceiverName = businessModel.ReceiverName;
            entity.ReceiverEMail = businessModel.ReceiverEmail;
            entity.ReceiverPhone = businessModel.ReceiverPhone;
            entity.ReceiverLocation = businessModel.ReceiverLocation;
            entity.MarkOfGoods = businessModel.MarkOfGoods;
        } 

        private static void MapSupplierFields(Order entity, SupplierEditFields businessModel)
        {
            entity.SupplierOrderNumber = businessModel.SupplierOrderNumber;
            entity.SupplierOrderDate = businessModel.SupplierOrderDate;
            entity.SupplierOrderInfo = businessModel.SupplierOrderInfo;
        } 

        private static void MapUserFields(Order entity, UserEditFields businessModel)
        {
            entity.UserId = businessModel.UserId;
            entity.UserFirstName = businessModel.UserFirstName;
            entity.UserLastName = businessModel.UserLastName;
            entity.UserPhone = businessModel.UserPhone;
            entity.UserEMail = businessModel.UserEMail;
            entity.UserInitials = businessModel.UserInitials;
            entity.InfoUser = businessModel.InfoUser;
            entity.Activity = businessModel.Activity;
            entity.UserDepartment_Id = businessModel.UserDepartment_Id1;
            entity.UserDepartment_Id2 = businessModel.UserDepartment_Id2;
            entity.EmploymentType = businessModel.EmploymentType;
            entity.UserExtension = businessModel.UserExtension;
            entity.UserLocation = businessModel.UserLocation;
            entity.Manager = businessModel.Manager;
            entity.UserPersonalIdentityNumber = businessModel.UserPersonalIdentityNumber;
            entity.UserPostalAddress = businessModel.UserPostalAddress;
            entity.ReferenceNumber = businessModel.ReferenceNumber;
            entity.Responsibility = businessModel.Responsibility;
            entity.UserRoomNumber = businessModel.UserRoomNumber;
            entity.UserTitle = businessModel.UserTitle;
            entity.UserOU_Id = businessModel.UserOU_Id;
        }
    }
}