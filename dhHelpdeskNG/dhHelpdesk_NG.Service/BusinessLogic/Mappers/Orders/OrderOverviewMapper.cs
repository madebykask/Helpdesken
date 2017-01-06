using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
using DH.Helpdesk.Common.Extensions.Boolean;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Orders;

    public static class OrderOverviewMapper
    {
        public static FullOrderOverview[] MapToFullOverviews(this IQueryable<Order> query,
            IList<OrderType> orderTypes,
            IList<Case> caseEntities,
            IList<OrderFieldType> orderFieldTypes)
        {
            var entities = query
                            .SelectIncluding(new List<Expression<Func<Order, object>>>
                                                {                                                    
                                                    o => o.Customer.Name,
                                                    o => o.Domain.Name,
                                                    o => o.Ou.Name,
                                                    o => o.OrderProperty.OrderProperty,
                                                    o => o.OrderState.Name,
                                                    o => o.OrderType.Name,
                                                    o => o.DeliveryDepartment.DepartmentName,
                                                    o => o.DeliveryOuEntity.Name,
                                                    o => o.Logs.Select(l => l.LogNote),
                                                    o => o.Programs.Select(p => p.Name),
                                                    o => o.User.FirstName,
                                                    o => o.User.SurName,
                                                    o => o.Department.DepartmentName,
                                                    o => o.UserOU.Name,
                                                    o => o.EmploymentType.Name,
                                                    o => o.UserDepartment1.DepartmentName,
                                                    o => o.UserDepartment2.DepartmentName
                                                })
                                                .ToArray();

            return entities.Select(
                o =>
                {
                        var order = (Order)o.sourceObject;
                    order.Customer = new Customer { Name = o.f0 };
                    order.Domain = new Domain { Name = o.f1 };
                    order.Ou = new OU { Name = o.f2 };
                    order.OrderProperty = new OrderPropertyEntity { OrderProperty = o.f3 };
                    order.OrderState = new OrderState { Name = o.f4 };
                    order.OrderType = new OrderType { Name = GetRootOrderTypeName(orderTypes, order.OrderType_Id) };
                    order.DeliveryDepartment = new Department { DepartmentName = o.f6 };
                    order.DeliveryOuEntity = new OU { Name = o.f7 };
                    order.Logs = ((List<string>)o.f8).Select(l => new OrderLog { LogNote = l }).ToArray();
                    order.Programs = ((List<string>)o.f9).Select(p => new Program { Name = p }).ToArray();
                    order.User = new User { FirstName = o.f10, SurName = o.f11 };
                    order.Department = new Department { DepartmentName = o.f12 };
                    order.UserOU = new OU { Name = o.f13 };
                    order.EmploymentType = new EmploymentType { Name = o.f14 };
                    order.UserDepartment1 = new Department { DepartmentName = o.f15 };
                    order.UserDepartment2 = new Department { DepartmentName = o.f16 };
                    return CreateFullOverview(order, caseEntities, orderFieldTypes);                    
                }).ToArray();
        }

        #region Create fields

        private static FullOrderOverview CreateFullOverview(Order entity,
            IList<Case> caseEntities,
            IList<OrderFieldType> orderFieldTypes)
        {
            var delivery = CreateDeliveryOverview(entity);
            var general = CreateGeneralOverview(entity);
            var log = CreateLogOverview(entity);
            var orderer = CreateOrdererOverview(entity);
            var order = CreateOrderOverview(entity);
            var curCase = caseEntities.Where(c=>  c.CaseNumber == entity.CaseNumber).SingleOrDefault();             
            var other = curCase == null ? CreateOtherOverview(entity) : CreateOtherOverview(entity, curCase);
            var program = CreateProgramOverview(entity);
            var receiver = CreateReceiverOverview(entity);
            var supplier = CreateSupplierOverview(entity);
            var user = CreateUserOverview(entity);
            var accountInfo = CreateAccountInfoOverview(entity, orderFieldTypes);
            var contact = CreateContactOverview(entity);

            return new FullOrderOverview(
                                    entity.Id,
                                    entity.OrderType,
                                    delivery,
                                    general,
                                    log,
                                    orderer,
                                    order,
                                    other,
                                    program,
                                    receiver,
                                    supplier,
                                    user,
                                    accountInfo,
                                    contact);
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
                                    entity.DeliveryOuEntity != null ? entity.DeliveryOuEntity.Name : string.Empty,
                                    entity.DeliveryName,
                                    entity.DeliveryPhone);
        }

        private static GeneralOverview CreateGeneralOverview(Order entity)
        {
            return new GeneralOverview(
                                    entity.Id.MapToOrderNumber(),
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

        private static OtherOverview CreateOtherOverview(Order entity, Case curCase = null)
        {
            return new OtherOverview(
                                    entity.Filename,
                                    entity.CaseNumber,
                                    curCase,
                                    entity.Info,
                                    entity.OrderState != null ? entity.OrderState.Name : string.Empty);
        }

        private static ProgramOverview CreateProgramOverview(Order entity)
        {
           return new ProgramOverview(entity.Programs.Select(p => p.Name).ToArray(), entity.InfoProduct);
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
                                    entity.UserOU != null ? entity.UserOU.Name : string.Empty,
                                    entity.EmploymentType != null ? entity.EmploymentType.Name : string.Empty,
                                    entity.UserDepartment1 != null ? entity.UserDepartment1.DepartmentName : string.Empty,
                                    entity.UserDepartment2 != null ? entity.UserDepartment2.DepartmentName : string.Empty);
        }

        private static AccountInfoOverview CreateAccountInfoOverview(Order entity,
            IList<OrderFieldType> orderFieldTypes)
        {
            var orderFieldType1 = "";
            var orderFieldType2 = "";
            var orderFieldType3 = "";
            var orderFieldType4 = "";
            var orderFieldType5 = "";
            if (entity.OrderFieldType_Id.HasValue ||
                string.IsNullOrEmpty(entity.OrderFieldType2) ||
                entity.OrderFieldType3_Id.HasValue ||
                entity.OrderFieldType4_Id.HasValue ||
                entity.OrderFieldType5_Id.HasValue)
            {
                if (entity.OrderFieldType_Id.HasValue)
                    orderFieldType1 = orderFieldTypes
                        .FirstOrDefault(o => o.OrderField == OrderFieldTypes.AccountType
                                             && o.Id == entity.OrderFieldType_Id.Value)?.Name;

                if (!string.IsNullOrEmpty(entity.OrderFieldType2))
                {
                    var orderFieldTypes2 = entity.OrderFieldType2.Split(',')
                        .Select(int.Parse).ToList();
                    orderFieldType2 = string.Join("; ", orderFieldTypes
                        .Where(o => o.OrderField == OrderFieldTypes.AccountType2
                                    && orderFieldTypes2.Any(i => i == o.Id))
                        .Select(o => o.Name).ToArray());
                }

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

            return new AccountInfoOverview(
                entity.AccountStartDate,
                entity.AccountEndDate,
                entity.EMailType,
                entity.HomeDirectory,
                entity.Profile,
                entity.InventoryNumber,
                orderFieldType1 ?? string.Empty,
                orderFieldType2 ?? string.Empty,
                orderFieldType3 ?? string.Empty,
                orderFieldType4 ?? string.Empty,
                orderFieldType5 ?? string.Empty,
                entity.AccountInfo);
        }

        private static ContactOverview CreateContactOverview(Order entity)
        {
            return new ContactOverview(
                entity.ContactId,
                entity.ContactName,
                entity.ContactPhone,
                entity.ContactPhone);
        }

        #endregion

        private static string GetRootOrderTypeName(IList<OrderType> orderTypes, int? id)
        {
            if (!id.HasValue)
                return "";

            for (var i = 0; i < 1000000; i++) //max 1M depth - to avoid infinite recursive call/loop in case of db incorrect data
            {
                var current = orderTypes.SingleOrDefault(x => x.Id == id);
                if (current == null)
                    return "";
                if (!current.Parent_OrderType_Id.HasValue)
                    return current.Name;
                id = current.Parent_OrderType_Id.Value;
            }

            return "";
        }
    }
}