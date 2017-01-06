namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderHistoryMapper
    {
        public static OrderHistoryModel MapToBusinessModel(UpdateOrderRequest request)
        {
            return OrderHistoryModel.CreateNew(
                    request.Order,
                    request.DateAndTime,
                    request.UserId);
        }

        public static OrderHistoryEntity MapToEntity(OrderHistoryModel businessModel)
        {
            var entity = new OrderHistoryEntity
                             {
                                 OrderHistoryGuid = Guid.NewGuid(),
                                 OrderId = businessModel.Order.Id,
                                 OrderType_Id = businessModel.Order.OrderTypeId.HasValue ? businessModel.Order.OrderTypeId.Value : 0,
                                 CreatedByUser_Id = businessModel.CreatedByUserId,
                                 CreatedDate = businessModel.CreatedDateAndTime
                             };

            MapDeliveryFields(entity, businessModel.Order.Delivery);
            MapGeneralFields(entity, businessModel.Order.General);
            MapLogFields(entity, businessModel.Order.Log);
            MapOrdererFields(entity, businessModel.Order.Orderer);
            MapOrderFields(entity, businessModel.Order.Order);
            MapOtherFields(entity, businessModel.Order.Other);
            MapProgramFields(entity, businessModel.Order.Program);
            MapReceiverFields(entity, businessModel.Order.Receiver);
            MapSupplierFields(entity, businessModel.Order.Supplier);
            MapUserFields(entity, businessModel.Order.User);
            MapAccountInfoFields(entity, businessModel.Order.AccountInfo);
            MapContactFields(entity, businessModel.Order.Contact);

            return entity;
        }

        private static void MapDeliveryFields(OrderHistoryEntity entity, DeliveryEditFields businessModel)
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
            entity.DeliveryName = businessModel.DeliveryName;
            entity.DeliveryPhone = businessModel.DeliveryPhone;
        }

        private static void MapGeneralFields(OrderHistoryEntity entity, GeneralEditFields businessModel)
        {
            entity.User_Id = businessModel.AdministratorId;
            entity.Domain_Id = businessModel.DomainId;
            entity.OrderDate = businessModel.OrderDate;
            entity.OrderState_Id = businessModel.StatusId;
        }

        private static void MapLogFields(OrderHistoryEntity entity, LogEditFields businessModel)
        {
        }

        private static void MapOrdererFields(OrderHistoryEntity entity, OrdererEditFields businessModel)
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

        private static void MapOrderFields(OrderHistoryEntity entity, OrderEditFields businessModel)
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

        private static void MapOtherFields(OrderHistoryEntity entity, OtherEditFields businessModel)
        {
            entity.Filename = businessModel.FileName;
            entity.CaseNumber = businessModel.CaseNumber;
            entity.Info = businessModel.Info;
        }

        private static void MapProgramFields(OrderHistoryEntity entity, ProgramEditFields businessModel)
        {
            entity.InfoProduct = businessModel.InfoProduct;
        }

        private static void MapReceiverFields(OrderHistoryEntity entity, ReceiverEditFields businessModel)
        {
            entity.ReceiverId = businessModel.ReceiverId;
            entity.ReceiverName = businessModel.ReceiverName;
            entity.ReceiverEMail = businessModel.ReceiverEmail;
            entity.ReceiverPhone = businessModel.ReceiverPhone;
            entity.ReceiverLocation = businessModel.ReceiverLocation;
            entity.MarkOfGoods = businessModel.MarkOfGoods;
        }

        private static void MapSupplierFields(OrderHistoryEntity entity, SupplierEditFields businessModel)
        {
            entity.SupplierOrderNumber = businessModel.SupplierOrderNumber;
            entity.SupplierOrderDate = businessModel.SupplierOrderDate;
            entity.SupplierOrderInfo = businessModel.SupplierOrderInfo;
        }

        private static void MapUserFields(OrderHistoryEntity entity, UserEditFields businessModel)
        {
            entity.UserId = businessModel.UserId;
            entity.UserFirstName = businessModel.UserFirstName;
            entity.UserLastName = businessModel.UserLastName;
            entity.UserEMail = businessModel.UserEMail;
            entity.UserPhone = businessModel.UserPhone;
            entity.UserInitials = businessModel.UserInitials;
            entity.UserPersonalIdentityNumber = businessModel.UserPersonalIdentityNumber;
            entity.UserExtension = businessModel.UserExtension;
            entity.UserTitle = businessModel.UserTitle;
            entity.UserLocation = businessModel.UserLocation;
            entity.UserRoomNumber = businessModel.UserRoomNumber;
            entity.UserPostalAddress = businessModel.UserPostalAddress;
            entity.Responsibility = businessModel.Responsibility;
            entity.Activity = businessModel.Activity;
            entity.Manager = businessModel.Manager;
            entity.ReferenceNumber = businessModel.ReferenceNumber;
            entity.InfoUser = businessModel.InfoUser;
            entity.UserOU_Id = businessModel.UserOU_Id;
            entity.EmploymentType_Id = businessModel.EmploymentType_Id;
            entity.UserDepartment_Id = businessModel.UserDepartment_Id1;
            entity.UserDepartment_Id2 = businessModel.UserDepartment_Id2;
        }

        private static void MapContactFields(OrderHistoryEntity entity, ContactEditFields businessModel)
        {
            entity.ContactId = businessModel.Id;
            entity.ContactName = businessModel.Name;
            entity.ContactPhone = businessModel.Phone;
            entity.ContactEMail = businessModel.Email;
        }

        private static void MapAccountInfoFields(OrderHistoryEntity entity, AccountInfoEditFields businessModel)
        {
            entity.AccountStartDate = businessModel.StartedDate;
            entity.AccountEndDate = businessModel.FinishDate;
            entity.EMailType = businessModel.EMailTypeId;
            entity.HomeDirectory = businessModel.HomeDirectory;
            entity.Profile = businessModel.Profile;
            entity.InventoryNumber = businessModel.InventoryNumber;
            entity.AccountInfo = businessModel.Info;
            entity.OrderFieldType_Id = businessModel.AccountTypeId;
            //entity.OrderFieldType2 = businessModel.Id;
            entity.OrderFieldType3_Id = businessModel.AccountTypeId3;
            entity.OrderFieldType4_Id = businessModel.AccountTypeId4;
            entity.OrderFieldType5_Id = businessModel.AccountTypeId5;
        }
    }
}