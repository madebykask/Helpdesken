namespace DH.Helpdesk.Services.Attributes.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Infrastructure;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingOrderFieldSettingsAttribute : OnMethodBoundaryAspect
    {
        private readonly string customerIdParameterName;

        private readonly string orderTypeIdParameterName;

        public CreateMissingOrderFieldSettingsAttribute(
                    string customerIdParameterName, 
                    string orderTypeIdParameterName)
        {
            this.customerIdParameterName = customerIdParameterName;
            this.orderTypeIdParameterName = orderTypeIdParameterName;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            
            var customerIdParameter = methodParameters.Single(p => p.Name == this.customerIdParameterName);
            var customerId = (int)args.Arguments[customerIdParameter.Position];

            var orderTypeIdParameter = methodParameters.Single(p => p.Name == this.orderTypeIdParameterName);
            var orderTypeId = (int?)args.Arguments[orderTypeIdParameter.Position];

            var unitOfWorkFactory = ManualDependencyResolver.Get<IUnitOfWorkFactory>();
            using (var uow = unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<OrderFieldSettings>();
                var existing = rep.GetAll()
                                .GetByType(customerId, orderTypeId)
                                .MapToFieldNames();

                var missing = new List<string>();
                CollectDeliveryMissingFields(existing, missing);
                CollectGeneralMissingFields(existing, missing);
                CollectLogMissingFields(existing, missing);
                CollectOrdererMissingFields(existing, missing);
                CollectOrderMissingFields(existing, missing);
                CollectOtherMissingFields(existing, missing);
                CollectProgramMissingFields(existing, missing);
                CollectReceiverMissingFields(existing, missing);
                CollectSupplierMissingFields(existing, missing);
                CollectUserMissingFields(existing, missing);
                CollectAccountInfoMissingFields(existing, missing);

                foreach (var fieldName in missing)
                {
                    var entity = CreateDefaultField(fieldName, customerId, orderTypeId);
                    rep.Add(entity);
                }

                uow.Save();
            }

            base.OnEntry(args);
        }

        private static void CollectDeliveryMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(DeliveryFields.DeliveryDate, existing, missingFields);
            CollectMissingField(DeliveryFields.InstallDate, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryDepartment, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryOu, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryAddress, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryPostalCode, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryPostalAddress, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryLocation, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryInfo1, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryInfo2, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryInfo3, existing, missingFields);
            CollectMissingField(DeliveryFields.DeliveryOuId, existing, missingFields);
        }

        private static void CollectGeneralMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(GeneralFields.OrderNumber, existing, missingFields);
            CollectMissingField(GeneralFields.Customer, existing, missingFields);
            CollectMissingField(GeneralFields.Administrator, existing, missingFields);
            CollectMissingField(GeneralFields.Domain, existing, missingFields);
            CollectMissingField(GeneralFields.OrderDate, existing, missingFields);
        }

        private static void CollectLogMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(LogFields.Log, existing, missingFields);
        }

        private static void CollectOrdererMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrdererFields.OrdererId, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererName, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererLocation, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererEmail, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererPhone, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererCode, existing, missingFields);
            CollectMissingField(OrdererFields.Department, existing, missingFields);
            CollectMissingField(OrdererFields.Unit, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererAddress, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererInvoiceAddress, existing, missingFields);
            CollectMissingField(OrdererFields.OrdererReferenceNumber, existing, missingFields);
            CollectMissingField(OrdererFields.AccountingDimension1, existing, missingFields);
            CollectMissingField(OrdererFields.AccountingDimension2, existing, missingFields);
            CollectMissingField(OrdererFields.AccountingDimension3, existing, missingFields);
            CollectMissingField(OrdererFields.AccountingDimension4, existing, missingFields);
            CollectMissingField(OrdererFields.AccountingDimension5, existing, missingFields);
        }

        private static void CollectOrderMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.Property, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow1, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow2, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow3, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow4, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow5, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow6, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow7, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow8, existing, missingFields);
            CollectMissingField(OrderFields.Configuration, existing, missingFields);
            CollectMissingField(OrderFields.OrderInfo, existing, missingFields);
            CollectMissingField(OrderFields.OrderInfo2, existing, missingFields);
        }

        private static void CollectOtherMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OtherFields.FileName, existing, missingFields);
            CollectMissingField(OtherFields.CaseNumber, existing, missingFields);
            CollectMissingField(OtherFields.Info, existing, missingFields);
            CollectMissingField(OtherFields.Status, existing, missingFields);
        }

        private static void CollectProgramMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(ProgramFields.Program, existing, missingFields);
        }

        private static void CollectReceiverMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(ReceiverFields.ReceiverId, existing, missingFields);
            CollectMissingField(ReceiverFields.ReceiverName, existing, missingFields);
            CollectMissingField(ReceiverFields.ReceiverEmail, existing, missingFields);
            CollectMissingField(ReceiverFields.ReceiverPhone, existing, missingFields);
            CollectMissingField(ReceiverFields.ReceiverLocation, existing, missingFields);
            CollectMissingField(ReceiverFields.MarkOfGoods, existing, missingFields);
        }

        private static void CollectSupplierMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(SupplierFields.SupplierOrderNumber, existing, missingFields);
            CollectMissingField(SupplierFields.SupplierOrderDate, existing, missingFields);
            CollectMissingField(SupplierFields.SupplierOrderInfo, existing, missingFields);
        }

        private static void CollectUserMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(UserFields.UserId, existing, missingFields);
            CollectMissingField(UserFields.UserFirstName, existing, missingFields);
            CollectMissingField(UserFields.UserLastName, existing, missingFields);
            CollectMissingField(UserFields.UserPhone, existing, missingFields);
            CollectMissingField(UserFields.UserEMail, existing, missingFields);
            CollectMissingField(UserFields.UserInitials, existing, missingFields);
            CollectMissingField(UserFields.InfoUser, existing, missingFields);
            CollectMissingField(UserFields.Activity, existing, missingFields);
            CollectMissingField(UserFields.UserDepartment_Id1, existing, missingFields);
            CollectMissingField(UserFields.UserDepartment_Id2, existing, missingFields);
            CollectMissingField(UserFields.EmploymentType, existing, missingFields);
            CollectMissingField(UserFields.UserExtension, existing, missingFields);
            CollectMissingField(UserFields.UserLocation, existing, missingFields);
            CollectMissingField(UserFields.Manager, existing, missingFields);
            CollectMissingField(UserFields.UserPersonalIdentityNumber, existing, missingFields);
            CollectMissingField(UserFields.UserPostalAddress, existing, missingFields);
            CollectMissingField(UserFields.ReferenceNumber, existing, missingFields);
            CollectMissingField(UserFields.Responsibility, existing, missingFields);
            CollectMissingField(UserFields.UserRoomNumber, existing, missingFields);
            CollectMissingField(UserFields.UserTitle, existing, missingFields);
            CollectMissingField(UserFields.UserOU_Id, existing, missingFields);
        }

        private static void CollectAccountInfoMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(AccountInfoFields.StartedDate, existing, missingFields);
            CollectMissingField(AccountInfoFields.FinishDate, existing, missingFields);
            CollectMissingField(AccountInfoFields.EMailTypeId, existing, missingFields);
            CollectMissingField(AccountInfoFields.HomeDirectory, existing, missingFields);
            CollectMissingField(AccountInfoFields.Profile, existing, missingFields);
            CollectMissingField(AccountInfoFields.InventoryNumber, existing, missingFields);
            CollectMissingField(AccountInfoFields.Info, existing, missingFields);
        }

        private static void CollectMissingField(
                        string fieldName,
                        string[] existing,
                        List<string> missingFields)
        {
            if (!existing.Any(f => string.Equals(f, fieldName, (StringComparison) StringComparison.CurrentCultureIgnoreCase)))
            {
                missingFields.Add(fieldName);
            }
        }

        private static OrderFieldSettings CreateDefaultField(
                                string fieldName,
                                int customerId,
                                int? orderTypeId)
        {
            var now = DateTime.Now;
            var visibleByDefault = (
                        fieldName == GeneralFields.OrderNumber ||
                        fieldName == GeneralFields.OrderDate ||
                        fieldName == OrdererFields.Department ||
                        fieldName == OrderFields.OrderRow1 ||
                        fieldName == ReceiverFields.ReceiverName ||
                        fieldName == SupplierFields.SupplierOrderNumber || 
                        fieldName == DeliveryFields.DeliveryDate ||
                        fieldName == OtherFields.CaseNumber ||
                        fieldName == OtherFields.Status)
                        .ToInt();
            return new OrderFieldSettings
                       {
                           OrderField = fieldName,
                           Customer_Id = customerId,
                           OrderType_Id = orderTypeId,
                           CreatedDate = now,
                           ChangedDate = now,
                           Label = fieldName,
                           Required = 0,
                           Show = visibleByDefault,
                           ShowExternal = 0,
                           ShowInList = visibleByDefault,
                           DefaultValue = string.Empty,
                           FieldHelp = string.Empty
                       };
        }
    }
}