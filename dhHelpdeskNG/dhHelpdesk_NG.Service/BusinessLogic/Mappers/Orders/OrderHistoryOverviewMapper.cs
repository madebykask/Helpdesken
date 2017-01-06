using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderHistoryOverviewMapper
    {
        public static List<HistoryOverview> MapToOverviews(this IQueryable<OrderHistoryEntity> query,
            IList<OrderFieldType> orderFieldTypes)
        {
            var entities = query
                            .SelectIncluding(new List<Expression<Func<OrderHistoryEntity, object>>>
                                                {                                                    
                                                    h => h.Domain.Name,
                                                    h => h.Ou.Name,
                                                    h => h.OrderProperty.OrderProperty,
                                                    h => h.OrderState.Name,
                                                    h => h.OrderType.Name,
                                                    h => h.DeliveryDepartment.DepartmentName,
                                                    h => h.Administrator.FirstName,
                                                    h => h.Administrator.SurName,
                                                    h => h.Department.DepartmentName,
                                                    h => h.CreatedByUser.FirstName,
                                                    h => h.CreatedByUser.SurName,
                                                    h => h.UserOU.Name,
                                                    h => h.EmploymentType.Name,
                                                    h => h.UserDepartment1.DepartmentName,
                                                    h => h.UserDepartment2.DepartmentName
                                                })
                                                .ToArray();

            return entities.Select(h =>
                    {
                        var history = (OrderHistoryEntity)h.sourceObject;
                        string domain = h.f0;
                        string ou = h.f1;
                        string orderProperty = h.f2;
                        string orderState = h.f3;
                        string orderType = h.f4;
                        string deliveryDepartment = h.f5;
                        var administrator = new UserName(h.f6, h.f7);
                        string department = h.f8;
                        var createdByUser = new UserName(h.f9, h.f10);
                        string userOU = h.f11;
                        string employmentType = h.f12;
                        string userDepartment1 = h.f13;
                        string userDepartment2 = h.f14;
                        return MapToOverview(
                                history, 
                                domain, 
                                ou, 
                                orderProperty, 
                                orderState, 
                                orderType, 
                                deliveryDepartment, 
                                administrator,
                                department,
                                createdByUser,
                                userOU,
                                employmentType,
                                userDepartment1,
                                userDepartment2,
                                orderFieldTypes);
                    }).ToList();
        }
        
        private static HistoryOverview MapToOverview(
                                OrderHistoryEntity entity,
                                string domain,
                                string ou,
                                string orderProperty,
                                string orderState,
                                string orderType,
                                string deliveryDepartment,
                                UserName administrator,
                                string department,
                                UserName createdByUser,
                                string userOuName,
                                string employmentTypeName,
                                string userDepartmentName,
                                string userDepartmentName2,
                                IList<OrderFieldType> orderFieldTypes)
        {
            var order = new FullOrderHistoryFields(            
                            MapDeliveryFields(entity, deliveryDepartment),
                            MapGeneralFields(entity, domain, administrator, orderState),
                            MapLogFields(entity),
                            MapOrdererFields(entity, department, ou),
                            MapOrderFields(entity, orderProperty),
                            MapOtherFields(entity, orderState),
                            MapProgramFields(entity),
                            MapReceiverFields(entity),
                            MapSupplierFields(entity),
                            MapUserFields(entity, userOuName, employmentTypeName, userDepartmentName, userDepartmentName2),
                            MapAccountInfoFields(entity, orderFieldTypes),
                            MapContactFields(entity));

            var historyOverview = new HistoryOverview(entity.Id, entity.CreatedDate, createdByUser, order);

            return historyOverview;
        }

        private static DeliveryHistoryFields MapDeliveryFields(OrderHistoryEntity entity, string deliveryDepartment)
        {
            return new DeliveryHistoryFields(
                entity.Deliverydate,
                entity.InstallDate,
                entity.DeliveryDepartmentId,
                deliveryDepartment,
                entity.DeliveryOu,
                entity.DeliveryAddress,
                entity.DeliveryPostalCode,
                entity.DeliveryPostalAddress,
                entity.DeliveryLocation,
                entity.DeliveryInfo,
                entity.DeliveryInfo2,
                entity.DeliveryInfo3,
                entity.DeliveryName,
                entity.DeliveryPhone);
        }

        private static GeneralHistoryFields MapGeneralFields(OrderHistoryEntity entity, string domain, UserName administrator, string status)
        {
            return new GeneralHistoryFields(
                entity.User_Id,
                administrator,
                entity.Domain_Id,
                domain,
                entity.OrderDate,
                entity.OrderState_Id,
                status);
        }

        private static LogHistoryFields MapLogFields(OrderHistoryEntity entity)
        {
            return new LogHistoryFields();
        }

        private static OrdererHistoryFields MapOrdererFields(OrderHistoryEntity entity, string department, string ou)
        {
            return new OrdererHistoryFields(
                entity.OrdererID,
                entity.Orderer,
                entity.OrdererLocation,
                entity.OrdererEMail,
                entity.OrdererPhone,
                entity.OrdererCode,
                entity.Department_Id,
                department,
                entity.OU_Id,
                ou,
                entity.OrdererAddress,
                entity.OrdererInvoiceAddress,
                entity.OrdererReferenceNumber,
                entity.AccountingDimension1,
                entity.AccountingDimension2,
                entity.AccountingDimension3,
                entity.AccountingDimension4,
                entity.AccountingDimension5);
        }

        private static OrderHistoryFields MapOrderFields(OrderHistoryEntity entity, string orderProperty)
        {
            return new OrderHistoryFields(
                entity.OrderPropertyId,
                orderProperty,
                entity.OrderRow,
                entity.OrderRow2,
                entity.OrderRow3,
                entity.OrderRow4,
                entity.OrderRow5,
                entity.OrderRow6,
                entity.OrderRow7,
                entity.OrderRow8,
                entity.Configuration,
                entity.OrderInfo,
                entity.OrderInfo2);
        }

        private static OtherHistoryFields MapOtherFields(OrderHistoryEntity entity, string orderState)
        {
            return new OtherHistoryFields(
                entity.Filename,
                entity.CaseNumber,
                entity.Info);
        }

        private static ProgramHistoryFields MapProgramFields(OrderHistoryEntity entity)
        {
            return new ProgramHistoryFields(new List<OrderProgramModel>(), entity.InfoProduct);
        }

        private static ReceiverHistoryFields MapReceiverFields(OrderHistoryEntity entity)
        {
            return new ReceiverHistoryFields(
                entity.ReceiverId,
                entity.ReceiverName,
                entity.ReceiverEMail,
                entity.ReceiverPhone,
                entity.ReceiverLocation,
                entity.MarkOfGoods);
        }

        private static SupplierHistoryFields MapSupplierFields(OrderHistoryEntity entity)
        {
            return new SupplierHistoryFields(
                entity.SupplierOrderNumber,
                entity.SupplierOrderDate,
                entity.SupplierOrderInfo);
        }

        private static UserHistoryFields MapUserFields(OrderHistoryEntity entity,
            string userOuName,
            string employmentTypeName,
            string userDepartmentName,
            string userDepartmentName2)
        {
            return new UserHistoryFields(
                entity.UserId,
                entity.UserFirstName,
                entity.UserLastName,
                entity.UserPhone,
                entity.UserEMail,
                entity.UserInitials,
                entity.UserPersonalIdentityNumber,
                entity.UserExtension,
                entity.UserTitle,
                entity.UserLocation,
                entity.UserRoomNumber,
                entity.UserPostalAddress,
                entity.Responsibility,
                entity.Activity,
                entity.Manager,
                entity.ReferenceNumber,
                entity.InfoUser,
                entity.UserOU_Id,
                userOuName,
                entity.EmploymentType_Id,
                employmentTypeName,
                entity.UserDepartment_Id,
                userDepartmentName,
                entity.UserDepartment_Id2,
                userDepartmentName2);
        }

        private static AccountInfoHistoryFields MapAccountInfoFields(OrderHistoryEntity entity,
            IList<OrderFieldType> orderFieldTypes)
        {
            var orderFieldType1 = "";
            //var orderFieldType2 = "";
            var orderFieldType3 = "";
            var orderFieldType4 = "";
            var orderFieldType5 = "";
            if (entity.OrderFieldType_Id.HasValue ||
                //string.IsNullOrEmpty(entity.OrderOrderFieldType2) ||
                entity.OrderFieldType3_Id.HasValue ||
                entity.OrderFieldType4_Id.HasValue ||
                entity.OrderFieldType5_Id.HasValue)
            {
                if (entity.OrderFieldType_Id.HasValue)
                    orderFieldType1 = orderFieldTypes
                        .FirstOrDefault(o => o.OrderField == OrderFieldTypes.AccountType
                                             && o.Id == entity.OrderFieldType_Id.Value)?.Name;

                //if (!string.IsNullOrEmpty(entity.OrderFieldType2))
                //{
                //    var orderFieldTypes2 = entity.OrderFieldType2.Split(',')
                //        .Select(int.Parse).ToList();
                //    orderFieldType2 = string.Join("; ", orderFieldTypes
                //        .Where(o => o.OrderField == OrderFieldTypes.AccountType2
                //                    && orderFieldTypes2.Any(i => i == o.Id))
                //        .Select(o => o.Name).ToArray());
                //}

                if (entity.OrderFieldType3_Id.HasValue)
                    orderFieldType3 = orderFieldTypes
                        .FirstOrDefault(o => o.OrderField == OrderFieldTypes.AccountType3
                                             && o.Id == entity.OrderFieldType3_Id.Value)?.Name;

                if (entity.OrderFieldType4_Id.HasValue)
                    orderFieldType4 = orderFieldTypes
                        .FirstOrDefault(o => o.OrderField == OrderFieldTypes.AccountType4
                                             && o.Id == entity.OrderFieldType4_Id.Value)?.Name;

                if (entity.OrderFieldType5_Id.HasValue)
                    orderFieldType5 = orderFieldTypes
                        .FirstOrDefault(o => o.OrderField == OrderFieldTypes.AccountType5
                                             && o.Id == entity.OrderFieldType5_Id.Value)?.Name;
            }


            return new AccountInfoHistoryFields(entity.AccountStartDate,
                entity.AccountEndDate,
                entity.EMailType,
                entity.HomeDirectory,
                entity.Profile,
                entity.InventoryNumber,
                entity.AccountInfo,
                entity.OrderFieldType_Id,
                orderFieldType1 ?? string.Empty,
                entity.OrderFieldType3_Id,
                orderFieldType3 ?? string.Empty,
                entity.OrderFieldType4_Id,
                orderFieldType4 ?? string.Empty,
                entity.OrderFieldType5_Id,
                orderFieldType5 ?? string.Empty);
        }

        private static ContactHistoryFields MapContactFields(OrderHistoryEntity entity)
        {
            return new ContactHistoryFields(
                entity.ContactId,
                entity.ContactName,
                entity.ContactPhone,
                entity.ContactEMail);
        }
    }
}