namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderHistoryOverviewMapper
    {
        public static List<HistoryOverview> MapToOverviews(this IQueryable<OrderHistoryEntity> query)
        {
            return new List<HistoryOverview>();
            /*var entities = query.ToList();

            return entities.Select(MapToEntity).ToList();*/
        }
        /*
        public static HistoryOverview MapToEntity(OrderHistoryEntity entity)
        {
            var order = new FullOrderHistoryFields(            
                            MapDeliveryFields(entity),
                            MapGeneralFields(entity),
                            MapLogFields(entity),
                            MapOrdererFields(entity),
                            MapOrderFields(entity),
                            MapOtherFields(entity),
                            MapProgramFields(entity),
                            MapReceiverFields(entity),
                            MapSupplierFields(entity),
                            MapUserFields(entity));
            
            var businessModel = new HistoryOverview();

            return businessModel;
        }

        private static DeliveryHistoryFields MapDeliveryFields(OrderHistoryEntity entity)
        {
            return new DeliveryHistoryFields(
                entity.Deliverydate,
                entity.InstallDate,
                entity.DeliveryDepartment != null ? entity.DeliveryDepartment.DepartmentName : string.Empty,
                entity.DeliveryOu,
                entity.DeliveryAddress,
                entity.DeliveryPostalCode,
                entity.DeliveryPostalAddress,
                entity.DeliveryLocation,
                entity.DeliveryInfo,
                entity.DeliveryInfo2,
                entity.DeliveryInfo3,
                string.Empty);
        }

        private static GeneralHistoryFields MapGeneralFields(OrderHistoryEntity entity)
        {
            return new GeneralHistoryFields(
                entity.User_Id,
                entity.Domain_Id,
                entity.OrderDate);
        }

        private static LogHistoryFields MapLogFields(OrderHistoryEntity entity)
        {
            return new LogHistoryFields();
        }

        private static OrdererHistoryFields MapOrdererFields(OrderHistoryEntity entity)
        {
            return new OrdererHistoryFields(
                entity.OrdererID,
                entity.Orderer,
                entity.OrdererLocation,
                entity.OrdererEMail,
                entity.OrdererPhone,
                entity.OrdererCode,
                entity.Department_Id,
                entity.OU_Id,
                entity.OrdererAddress,
                entity.OrdererInvoiceAddress,
                entity.OrdererReferenceNumber,
                entity.AccountingDimension1,
                entity.AccountingDimension2,
                entity.AccountingDimension3,
                entity.AccountingDimension4,
                entity.AccountingDimension5);
        }

        private static OrderHistoryFields MapOrderFields(OrderHistoryEntity entity)
        {
            return new OrderHistoryFields(
                entity.OrderPropertyId,
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

        private static OtherHistoryFields MapOtherFields(OrderHistoryEntity entity)
        {
            return new OtherHistoryFields(
                entity.Filename,
                entity.CaseNumber,
                entity.Info,
                entity.OrderState_Id);
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
        }*/ 
    }
}