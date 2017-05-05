using System.Reflection;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Services.utils;

namespace DH.Helpdesk.Services.Attributes.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BusinessData.Enums.Orders.Fields;
    using Common.Extensions.Boolean;
    using Dal.NewInfrastructure;
    using Domain;
    using BusinessLogic.Mappers.Orders;
    using BusinessLogic.Specifications.Orders;
    using Infrastructure;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingOrderFieldSettingsAttribute : OnMethodBoundaryAspect
    {
        private readonly string _customerIdParameterName;

        private readonly string _orderTypeIdParameterName;

        public CreateMissingOrderFieldSettingsAttribute(
                    string customerIdParameterName, 
                    string orderTypeIdParameterName)
        {
            _customerIdParameterName = customerIdParameterName;
            _orderTypeIdParameterName = orderTypeIdParameterName;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            
            var customerIdParameter = methodParameters.Single(p => p.Name == _customerIdParameterName);
            var customerId = (int)args.Arguments[customerIdParameter.Position];

            var orderTypeIdParameter = methodParameters.Single(p => p.Name == _orderTypeIdParameterName);
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
                CollectContactMissingFields(existing, missing);

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
            CollectMissingField(OrderFields.DeliveryDate, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryInstallDate, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryDepartment, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryOu, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryAddress, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryPostalCode, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryPostalAddress, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryLocation, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryInfo1, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryInfo2, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryInfo3, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryOuId, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryName, existing, missingFields);
            CollectMissingField(OrderFields.DeliveryPhone, existing, missingFields);
        }

        private static void CollectGeneralMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.GeneralOrderNumber, existing, missingFields);
            CollectMissingField(OrderFields.GeneralCustomer, existing, missingFields);
            CollectMissingField(OrderFields.GeneralAdministrator, existing, missingFields);
            CollectMissingField(OrderFields.GeneralDomain, existing, missingFields);
            CollectMissingField(OrderFields.GeneralOrderDate, existing, missingFields);
        }

        private static void CollectLogMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.Log, existing, missingFields);
        }

        private static void CollectOrdererMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.OrdererId, existing, missingFields);
            CollectMissingField(OrderFields.OrdererName, existing, missingFields);
            CollectMissingField(OrderFields.OrdererLocation, existing, missingFields);
            CollectMissingField(OrderFields.OrdererEmail, existing, missingFields);
            CollectMissingField(OrderFields.OrdererPhone, existing, missingFields);
            CollectMissingField(OrderFields.OrdererCode, existing, missingFields);
            CollectMissingField(OrderFields.OrdererDepartment, existing, missingFields);
            CollectMissingField(OrderFields.OrdererUnit, existing, missingFields);
            CollectMissingField(OrderFields.OrdererAddress, existing, missingFields);
            CollectMissingField(OrderFields.OrdererInvoiceAddress, existing, missingFields);
            CollectMissingField(OrderFields.OrdererReferenceNumber, existing, missingFields);
            CollectMissingField(OrderFields.OrdererAccountingDimension1, existing, missingFields);
            CollectMissingField(OrderFields.OrdererAccountingDimension2, existing, missingFields);
            CollectMissingField(OrderFields.OrdererAccountingDimension3, existing, missingFields);
            CollectMissingField(OrderFields.OrdererAccountingDimension4, existing, missingFields);
            CollectMissingField(OrderFields.OrdererAccountingDimension5, existing, missingFields);
        }

        private static void CollectOrderMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.OrderProperty, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow1, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow2, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow3, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow4, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow5, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow6, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow7, existing, missingFields);
            CollectMissingField(OrderFields.OrderRow8, existing, missingFields);
            CollectMissingField(OrderFields.OrderConfiguration, existing, missingFields);
            CollectMissingField(OrderFields.OrderInfo, existing, missingFields);
            CollectMissingField(OrderFields.OrderInfo2, existing, missingFields);
        }

        private static void CollectOtherMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.OtherFileName, existing, missingFields);
            CollectMissingField(OrderFields.OtherCaseNumber, existing, missingFields);
            CollectMissingField(OrderFields.OtherInfo, existing, missingFields);
            CollectMissingField(OrderFields.OtherStatus, existing, missingFields);
        }

        private static void CollectProgramMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.Program, existing, missingFields);
            CollectMissingField(OrderFields.ProgramInfoProduct, existing, missingFields);
        }

        private static void CollectReceiverMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.ReceiverId, existing, missingFields);
            CollectMissingField(OrderFields.ReceiverName, existing, missingFields);
            CollectMissingField(OrderFields.ReceiverEmail, existing, missingFields);
            CollectMissingField(OrderFields.ReceiverPhone, existing, missingFields);
            CollectMissingField(OrderFields.ReceiverLocation, existing, missingFields);
            CollectMissingField(OrderFields.ReceiverMarkOfGoods, existing, missingFields);
        }

        private static void CollectSupplierMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.SupplierOrderNumber, existing, missingFields);
            CollectMissingField(OrderFields.SupplierOrderDate, existing, missingFields);
            CollectMissingField(OrderFields.SupplierOrderInfo, existing, missingFields);
        }

        private static void CollectUserMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.UserId, existing, missingFields);
            CollectMissingField(OrderFields.UserFirstName, existing, missingFields);
            CollectMissingField(OrderFields.UserLastName, existing, missingFields);
            CollectMissingField(OrderFields.UserPhone, existing, missingFields);
            CollectMissingField(OrderFields.UserEMail, existing, missingFields);
            CollectMissingField(OrderFields.UserInitials, existing, missingFields);
            CollectMissingField(OrderFields.UserInfo, existing, missingFields);
            CollectMissingField(OrderFields.UserActivity, existing, missingFields);
            CollectMissingField(OrderFields.UserDepartment_Id1, existing, missingFields);
            CollectMissingField(OrderFields.UserDepartment_Id2, existing, missingFields);
            CollectMissingField(OrderFields.UserEmploymentType, existing, missingFields);
            CollectMissingField(OrderFields.UserExtension, existing, missingFields);
            CollectMissingField(OrderFields.UserLocation, existing, missingFields);
            CollectMissingField(OrderFields.UserManager, existing, missingFields);
            CollectMissingField(OrderFields.UserPersonalIdentityNumber, existing, missingFields);
            CollectMissingField(OrderFields.UserPostalAddress, existing, missingFields);
            CollectMissingField(OrderFields.UserReferenceNumber, existing, missingFields);
            CollectMissingField(OrderFields.UserResponsibility, existing, missingFields);
            CollectMissingField(OrderFields.UserRoomNumber, existing, missingFields);
            CollectMissingField(OrderFields.UserTitle, existing, missingFields);
            CollectMissingField(OrderFields.UserOU_Id, existing, missingFields);
        }

        private static void CollectAccountInfoMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.AccountInfoStartedDate, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoFinishDate, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoEMailTypeId, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoHomeDirectory, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoProfile, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoInventoryNumber, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfo, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoAccountType, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoAccountType2, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoAccountType3, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoAccountType4, existing, missingFields);
            CollectMissingField(OrderFields.AccountInfoAccountType5, existing, missingFields);
        }

        private static void CollectContactMissingFields(string[] existing, List<string> missingFields)
        {
            CollectMissingField(OrderFields.ContactId, existing, missingFields);
            CollectMissingField(OrderFields.ContactName, existing, missingFields);
            CollectMissingField(OrderFields.ContactPhone, existing, missingFields);
            CollectMissingField(OrderFields.ContactEMail, existing, missingFields);
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
            var label = GetFieldNameDefaultLabel(fieldName);
            var visibleByDefault = (
                        fieldName == OrderFields.GeneralOrderNumber ||
                        fieldName == OrderFields.GeneralOrderDate ||
                        fieldName == OrderFields.OrdererDepartment ||
                        fieldName == OrderFields.OrderRow1 ||
                        fieldName == OrderFields.ReceiverName ||
                        fieldName == OrderFields.SupplierOrderNumber || 
                        fieldName == OrderFields.DeliveryDate ||
                        fieldName == OrderFields.OtherCaseNumber ||
                        fieldName == OrderFields.OtherStatus)
                        .ToInt();
            return new OrderFieldSettings
                       {
                           OrderField = fieldName,
                           Customer_Id = customerId,
                           OrderType_Id = orderTypeId,
                           CreatedDate = now,
                           ChangedDate = now,
                           Label = label,
                           Required = 0,
                           Show = visibleByDefault,
                           ShowExternal = 0,
                           ShowInList = visibleByDefault,
                           DefaultValue = string.Empty,
                           FieldHelp = string.Empty,
                           MultiValue = false
                        };
        }

        private static string GetFieldNameDefaultLabel(string fieldName)
        {
            string constantName = null;
            string fieldLabel = null;

            foreach (var orderField in typeof(OrderFields).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (orderField.FieldType == typeof(string) && fieldName.Equals((string)orderField.GetValue(null)))
                {
                    constantName = orderField.Name;
                }
            }

            if (constantName != null)
            {
                foreach (var orderLabel in typeof(OrderLabels).GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    if (orderLabel.FieldType == typeof(string) && orderLabel.Name.Equals(constantName))
                    {
                        fieldLabel = (string) orderLabel.GetValue(null);
                    }
                }
            }
            return fieldLabel ?? fieldName;
        }
    }
}