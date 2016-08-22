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
        public static List<HistoryOverview> MapToOverviews(this IQueryable<OrderHistoryEntity> query)
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
                                                    h => h.CreatedByUser.SurName
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
                                createdByUser);
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
                                UserName createdByUser)
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
                            MapUserFields(entity));
            
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
                entity.DeliveryInfo3);
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
            return new ProgramHistoryFields();
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

        private static UserHistoryFields MapUserFields(OrderHistoryEntity entity)
        {
            return new UserHistoryFields(
                entity.UserId,
                entity.UserFirstName,
                entity.UserLastName);
        } 
    }
}