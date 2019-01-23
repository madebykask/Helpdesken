using System.Web.Mvc;
using Antlr.Runtime.Misc;
using DH.Helpdesk.Domain.Orders;
using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public class UpdateOrderModelFactory : IUpdateOrderModelFactory
    {
        public UpdateOrderRequest Create(
                        FullOrderEditModel model, 
                        int customerId, 
                        DateTime dateAndTime, 
                        IEmailService emailService, 
                        int userId,
                        int languageId)
        {
            int orderId;
            int.TryParse(model.Id, out orderId);

            var fields = new FullOrderEditFields(
                orderId,
                model.OrderTypeId,
                CreateDeliveryFields(model.Delivery),
                CreateGeneralFields(model.General),
                CreateLogFields(model.Log),
                CreateOrdererFields(model.UserInfo),
                CreateOrderFields(model.Order),
                CreateOtherFields(model.Other, model.NewFiles, model.DeletedFiles),
                CreateProgramFields(model.Program),
                CreateReceiverFields(model.Receiver),
                CreateSupplierFields(model.Supplier),
                CreateUserFields(model.User, model.UserInfo),
                CreateAccountInfoFields(model.Order));

            var newLogs = CreateNewLogCollection(model, emailService);

            return new UpdateOrderRequest(
                                fields, 
                                customerId, 
                                dateAndTime, 
                                model.DeletedLogIds, 
                                newLogs, 
                                userId,
                                model.InformOrderer,
                                model.InformReceiver,
                                model.CreateCase,
                                languageId);
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
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.DeliveryDepartment),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryOu),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryAddress),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryPostalCode),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryPostalAddress),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryLocation),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryInfo1),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryInfo2),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryInfo3),
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.DeliveryOuId),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryName),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.DeliveryPhone));
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
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.Administrator),
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.Domain),
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.OrderDate),
                    model.StatusId);
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

        private static OrdererEditFields CreateOrdererFields(UserInfoEditModel model)
        {
            if (model == null)
            {
                model = UserInfoEditModel.CreateEmpty();
            }

            return new OrdererEditFields(
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererId),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererName),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererLocation),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererEmail),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererPhone),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.OrdererCode),
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.DepartmentId1),
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.Unit),
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
                    ConfigurableFieldModel<int?>.GetValueOrDefault(model.Property),
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

            var fileName = newFiles.Select(f => f.Name).FirstOrDefault() ?? string.Empty;

            return new OtherEditFields(
                    fileName,
                    ConfigurableFieldModel<decimal?>.GetValueOrDefault(model.CaseNumber),
                    ConfigurableFieldModel<string>.GetValueOrDefault(model.Info));
        }

        private static ProgramEditFields CreateProgramFields(ProgramEditModel model)
        {
            if (model == null) model = ProgramEditModel.CreateEmpty();

            var programs = model.Programs?.Value?.Where(x => x.IsChecked).Select(p => p.Id).ToList();
            var infoProduct = ConfigurableFieldModel<string>.GetValueOrDefault(model.InfoProduct);

            return new ProgramEditFields(programs ?? new List<int>(), infoProduct);
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

        private static UserEditFields CreateUserFields(UserEditModel model, UserInfoEditModel infoModel)
        {
            if (model == null) model = UserEditModel.CreateEmpty();
            if (infoModel == null) infoModel = UserInfoEditModel.CreateEmpty();

            return new UserEditFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.UserId),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.UserFirstName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.UserLastName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.UserPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.UserEMail),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Initials),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.PersonalIdentityNumber),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Extension),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Title),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Location),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.RoomNumber),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.PostalAddress),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Responsibility),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Activity),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Manager),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.ReferenceNumber),
                ConfigurableFieldModel<string>.GetValueOrDefault(infoModel.Info),
                ConfigurableFieldModel<int?>.GetValueOrDefault(infoModel.Unit),
                ConfigurableFieldModel<int?>.GetValueOrDefault(infoModel.EmploymentTypeId),
                ConfigurableFieldModel<int?>.GetValueOrDefault(infoModel.DepartmentId1),
                ConfigurableFieldModel<int?>.GetValueOrDefault(infoModel.DepartmentId2),
                infoModel.RegionId);
        }

        private static AccountInfoEditFields CreateAccountInfoFields(OrderEditModel model)
        {
            if (model == null) model = OrderEditModel.CreateEmpty();

            return new AccountInfoEditFields(
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.StartedDate),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishDate),
                (EMailTypes?)ConfigurableFieldModel<int?>.GetValueOrDefault(model.EMailTypeId),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.HomeDirectory),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Profile),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.InventoryNumber),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Info),
                ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountTypeId),
                ConfigurableFieldModel<List<int>>.GetValueOrDefault(model.AccountTypeId2),
                ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountTypeId3),
                ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountTypeId4),
                ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountTypeId5));
        }

        private static List<ManualLog> CreateNewLogCollection(FullOrderEditModel model, IEmailService emailService)
        {
            var newLogs = new List<ManualLog>();

            if (model.Log != null)
            {
                CreateNewLogIfNeeded(model.Log.Log, Subtopic.Log, newLogs, emailService);                
            }

            return newLogs;
        }

        private static void CreateNewLogIfNeeded(
                    ConfigurableFieldModel<LogsModel> logField,
                    Subtopic subtopic,
                    List<ManualLog> logs,
                    IEmailService emailService)
        {
            if (logField == null || string.IsNullOrEmpty(logField.Value.Text))
            {
                return;
            }

            var emails = string.IsNullOrEmpty(logField.Value.Emails)
                ? new List<MailAddress>(0)
                : logField.Value.Emails.Split(Environment.NewLine)
                                .Where(emailService.IsValidEmail)
                                .Select(e => new MailAddress(e))
                                .ToList();

            var newLog = ManualLog.CreateNew(logField.Value.Text, emails, subtopic);
            logs.Add(newLog);
        }
    }
}