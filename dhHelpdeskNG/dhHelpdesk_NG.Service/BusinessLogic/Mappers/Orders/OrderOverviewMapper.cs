namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;

    public static class OrderOverviewMapper
    {
        public static FullOrderOverview[] MapToFullOverviews(this IQueryable<Order> query)
        {
            var entities = query
                            .IncludePath(o => o.Customer)
                            .IncludePath(o => o.Domain)
                            .IncludePath(o => o.Ou)
                            .IncludePath(o => o.OrderProperty)
                            .IncludePath(o => o.OrderState)
                            .IncludePath(o => o.OrderType)
                            .IncludePath(o => o.DeliveryDepartment)
                            .IncludePath(o => o.DeliveryOU)
                            .IncludePath(o => o.Logs)
                            .IncludePath(o => o.Programs)
                            .ToArray();

            return entities.Select(CreateFullOverview).ToArray();
        }

        #region Create fields

        private static FullOrderOverview CreateFullOverview(Order entity)
        {
            var delivery = CreateDeliveryOverview(entity);
            var general = CreateGeneralOverview(entity);
            var log = CreateLogOverview(entity);
            var orderer = CreateOrdererOverview(entity);
            var order = CreateOrderOverview(entity);
            var other = CreateOtherOverview(entity);
            var program = CreateProgramOverview(entity);
            var receiver = CreateReceiverOverview(entity);
            var supplier = CreateSupplierOverview(entity);
            var user = CreateUserOverview(entity);

            return new FullOrderOverview(
                                    entity.Id,
                                    delivery,
                                    general,
                                    log,
                                    orderer,
                                    order,
                                    other,
                                    program,
                                    receiver,
                                    supplier,
                                    user);
        }

        private static DeliveryOverview CreateDeliveryOverview(Order entity)
        {
            return new DeliveryOverview(
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
                                    entity.DeliveryOU != null ? entity.DeliveryOU.Name : string.Empty);
        }

        private static GeneralOverview CreateGeneralOverview(Order entity)
        {
            return new GeneralOverview(
                                    string.Format("O-{0}", entity.Id),
                                    entity.Customer != null ? entity.Customer.Name : string.Empty,
                                    entity.User != null ? string.Format("{0} {1}", entity.User.FirstName, entity.User.SurName) : string.Empty,
                                    entity.Domain != null ? entity.Domain.Name : string.Empty,
                                    entity.OrderDate);
        }

        private static LogOverview CreateLogOverview(Order entity)
        {
            return new LogOverview(entity.Logs.Select(l => l.LogNote).ToArray());
        }

        private static OrdererOverview CreateOrdererOverview(Order entity)
        {
            return new OrdererOverview(
                                    entity.OrdererID,
                                    entity.Orderer,
                                    entity.OrdererLocation,
                                    entity.OrdererEMail,
                                    entity.OrdererPhone,
                                    entity.OrdererCode,
                                    entity.Department != null ? entity.Department.DepartmentName : string.Empty,
                                    entity.Ou != null ? entity.Ou.Name : string.Empty,
                                    entity.OrdererAddress,
                                    entity.OrdererInvoiceAddress,
                                    entity.OrdererReferenceNumber,
                                    entity.AccountingDimension1,
                                    entity.AccountingDimension2,
                                    entity.AccountingDimension3,
                                    entity.AccountingDimension4,
                                    entity.AccountingDimension5);
        }

        private static OrderOverview CreateOrderOverview(Order entity)
        {
            return new OrderOverview(
                                    entity.OrderProperty != null ? entity.OrderProperty.OrderProperty : string.Empty,
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
                                    entity.OrderInfo2.ToString(CultureInfo.InvariantCulture));
        }

        private static OtherOverview CreateOtherOverview(Order entity)
        {
            return new OtherOverview(
                                    entity.Filename,
                                    entity.CaseNumber,
                                    entity.Info,
                                    entity.OrderState != null ? entity.OrderState.Name : string.Empty);
        }

        private static ProgramOverview CreateProgramOverview(Order entity)
        {
           return new ProgramOverview(entity.Programs.Select(p => p.Name).ToArray());
        }

        private static ReceiverOverview CreateReceiverOverview(Order entity)
        {
            return new ReceiverOverview(
                                    entity.ReceiverId,
                                    entity.ReceiverName,
                                    entity.ReceiverEMail,
                                    entity.ReceiverPhone,
                                    entity.ReceiverLocation,
                                    entity.MarkOfGoods);
        }

        private static SupplierOverview CreateSupplierOverview(Order entity)
        {
            return new SupplierOverview(
                                    entity.SupplierOrderNumber,
                                    entity.SupplierOrderDate,
                                    entity.SupplierOrderInfo);
        }

        private static UserOverview CreateUserOverview(Order entity)
        {
            return new UserOverview(
                                    entity.UserId,
                                    entity.UserFirstName,
                                    entity.UserLastName);
        }

        #endregion
    }
}