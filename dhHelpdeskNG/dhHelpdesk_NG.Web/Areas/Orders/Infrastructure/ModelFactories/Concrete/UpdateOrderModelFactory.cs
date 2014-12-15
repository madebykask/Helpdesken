namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public class UpdateOrderModelFactory : IUpdateOrderModelFactory
    {
        public UpdateOrderRequest Create(FullOrderEditModel model, int customerId, DateTime dateAndTime)
        {
            int orderId;
            int.TryParse(model.Id, out orderId);

            var fields = new FullOrderEditFields(
                orderId,
                model.OrderTypeId,
                CreateDeliveryFields(model.Delivery),
                CreateGeneralFields(model.General),
                CreateLogFields(model.Log),
                CreateOrdererFields(model.Orderer),
                CreateOrderFields(model.Order),
                CreateOtherFields(model.Other, model.NewFiles, model.DeletedFiles),
                CreateProgramFields(model.Program),
                CreateReceiverFields(model.Receiver),
                CreateSupplierFields(model.Supplier),
                CreateUserFields(model.User));

            return new UpdateOrderRequest(fields, customerId, dateAndTime);
        }

        private static DeliveryEditFields CreateDeliveryFields(DeliveryEditModel model)
        {
            if (model == null)
            {
                model = DeliveryEditModel.CreateEmpty();
            }

            return new DeliveryEditFields(
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.DeliveryDate),
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.InstallDate),
                    model.DeliveryDepartmentId,
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryOu),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryAddress),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryPostalCode),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryPostalAddress),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryLocation),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryInfo1),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryInfo2),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryInfo3),
                    model.DeliveryOuIdId);
        }

        private static GeneralEditFields CreateGeneralFields(GeneralEditModel model)
        {
            if (model == null)
            {
                model = GeneralEditModel.CreateEmpty();
            }

            return new GeneralEditFields(
                    ConfigurableFieldModel<int>.GetValueOrDefault(model.OrderNumber),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.Customer),
                    model.AdministratorId,
                    model.DomainId,
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.OrderDate));
        }

        private static LogEditFields CreateLogFields(LogEditModel model)
        {
            if (model == null)
            {
                model = LogEditModel.CreateEmpty();
            }

            var logs = model.Log.Value.Logs.Select(l => new Log(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            return new LogEditFields(logs);
        }

        private static OrdererEditFields CreateOrdererFields(OrdererEditModel model)
        {
            if (model == null)
            {
                model = OrdererEditModel.CreateEmpty();
            }

            return new OrdererEditFields(
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererId),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererName),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererLocation),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererEmail),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererPhone),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererCode),
                    model.DepartmentId,
                    model.UnitId,
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererAddress),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererInvoiceAddress),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererReferenceNumber),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.AccountingDimension1),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.AccountingDimension2),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.AccountingDimension3),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.AccountingDimension4),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.AccountingDimension5));
        }

        private static OrderEditFields CreateOrderFields(OrderEditModel model)
        {
            if (model == null)
            {
                model = OrderEditModel.CreateEmpty();
            }

            return new OrderEditFields(
                    model.PropertyId,
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow1),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow2),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow3),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow4),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow5),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow6),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow7),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderRow8),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.Configuration),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrderInfo),
                    ConfigurableFieldModel<int>.GetValueOrDefault(model.OrderInfo2));
        }

        private static OtherEditFields CreateOtherFields(OtherEditModel model, IEnumerable<WebTemporaryFile> newFiles, IEnumerable<string> deletedFiles)
        {
            if (model == null)
            {
                model = OtherEditModel.CreateEmpty();
            }

            return new OtherEditFields(
                    !deletedFiles.Any() ? newFiles.Select(f => f.Name).FirstOrDefault() : string.Empty,
                    ConfigurableFieldModel<decimal?>.GetValueOrDefault(model.CaseNumber),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.Info),
                    model.StatusId);
        }

        private static ProgramEditFields CreateProgramFields(ProgramEditModel model)
        {
            if (model == null)
            {
                model = ProgramEditModel.CreateEmpty();
            }

            var programs = model.Program.Value.Programs.Select(p => new OrderProgramModel(p.Id, p.Name)).ToList();
            return new ProgramEditFields(programs);
        }

        private static ReceiverEditFields CreateReceiverFields(ReceiverEditModel model)
        {
            if (model == null)
            {
                model = ReceiverEditModel.CreateEmpty();
            }

            return new ReceiverEditFields(
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.ReceiverId),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.ReceiverName),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.ReceiverEmail),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.ReceiverPhone),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.ReceiverLocation),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.MarkOfGoods));
        }

        private static SupplierEditFields CreateSupplierFields(SupplierEditModel model)
        {
            if (model == null)
            {
                model = SupplierEditModel.CreateEmpty();
            }

            return new SupplierEditFields(
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.SupplierOrderNumber),
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.SupplierOrderDate),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.SupplierOrderInfo));
        }

        private static UserEditFields CreateUserFields(UserEditModel model)
        {
            if (model == null)
            {
                model = UserEditModel.CreateEmpty();
            }

            return new UserEditFields(
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.UserId),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.UserFirstName),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.UserLastName));
        }
    }
}