namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Shared.Data;

    public static class OrderEditMapper
    {
        public static OrderEditOptions MapToOrderEditOptions(
                                        string orderTypeName,
                                        IQueryable<OrderState> statuses,
                                        IQueryable<User> administrators,
                                        IQueryable<Domain> domains,
                                        IQueryable<Department> departments,
                                        IQueryable<OU> units,
                                        IQueryable<OrderPropertyEntity> properties,
                                        IQueryable<Department> deliveryDepartments,
                                        IQueryable<OU> deliveryOuIds,
                                        List<GroupWithEmails> emailGroups,
                                        List<GroupWithEmails> workingGroupsWithEmails, 
                                        IQueryable<User> administratorsWithEmails,
                                        FullOrderEditSettings settings)
        {
            IQueryable<UnionItemOverview> query = null;

            if (settings.Other.Status.Show)
            {
                query = statuses.Select(s => new UnionItemOverview { Id = s.Id, Name = s.Name, Type = "statuses" });
            }

            if (settings.General.Administrator.Show)
            {
                var union = administrators.Select(a => new UnionItemOverview { Id = a.Id, Name = a.FirstName + " " + a.SurName, Type = "administrators" });
                query = query == null ? union : query.Union(union);
            }

            if (settings.General.Domain.Show)
            {
                var union = domains.Select(d => new UnionItemOverview { Id = d.Id, Name = d.Name, Type = "domains" });
                query = query == null ? union : query.Union(union);
            }

            if (settings.Orderer.Department.Show)
            {
                var union = departments.Select(d => new UnionItemOverview { Id = d.Id, Name = d.DepartmentName, Type = "departments" });
                query = query == null ? union : query.Union(union);
            }

            if (settings.Orderer.Unit.Show)
            {
                var union = units.Select(u => new UnionItemOverview { Id = u.Id, Name = u.Name, Type = "units" });
                query = query == null ? union : query.Union(union);
            }

            var propertiesOverviews = new ItemOverview[0];
            if (settings.Order.Property.Show)
            {
                propertiesOverviews = properties.MapToItemOverviews();
            }

            if (settings.Delivery.DeliveryDepartment.Show)
            {
                var union = deliveryDepartments.Select(d => new UnionItemOverview { Id = d.Id, Name = d.DepartmentName, Type = "deliveryDepartments" });
                query = query == null ? union : query.Union(union);
            }

            if (settings.Delivery.DeliveryOuId.Show)
            {
                var union = deliveryDepartments.Select(u => new UnionItemOverview { Id = u.Id, Name = u.DepartmentName, Type = "deliveryOuIds" });
                query = query == null ? union : query.Union(union);
            }

            var separator = Guid.NewGuid().ToString();

            if (settings.Log.Log.Show)
            {
                var union = administratorsWithEmails.Select(a => new UnionItemOverview { Id = a.Id, Name = a.FirstName + " " + a.SurName + separator + a.Email, Type = "administratorsWithEmails" });
                query = query == null ? union : query.Union(union);
            }

            var overviews = new UnionItemOverview[0];

            if (query != null)
            {
                overviews = query
                    .OrderBy(u => u.Type)
                    .ThenBy(u => u.Name)
                    .ToArray();
            }

            return new OrderEditOptions(
                orderTypeName,
                overviews.Where(o => o.Type == "statuses").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "administrators").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "domains").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "departments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "units").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                propertiesOverviews,
                overviews.Where(o => o.Type == "deliveryDepartments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "deliveryOuIds").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                emailGroups,
                workingGroupsWithEmails,
                overviews.Where(o => o.Type == "administratorsWithEmails").Select(a => { 
                            var values = a.Name.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                            return new ItemOverview(values[0], values.Length > 1 ? values[1].Split(';').First() : string.Empty);
                        }).ToList());
        }

        public static FullOrderEditFields MapToFullOrderEditFields(this IQueryable<Order> query)
        {
            var entity = query.Single();

            return new FullOrderEditFields(
                    entity.Id,
                    entity.OrderType_Id,
                    CreateDeliveryEditFields(entity),
                    CreateGeneralEditFields(entity),
                    CreateLogEditFields(entity),
                    CreateOrdererEditFields(entity),
                    CreateOrderEditFields(entity),
                    CreateOtherEditFields(entity),
                    CreateProgramEditFields(entity),
                    CreateReceiverEditFields(entity),
                    CreateSupplierEditFields(entity),
                    CreateUserEditFields(entity));
        }

        private static DeliveryEditFields CreateDeliveryEditFields(Order entity)
        {
            return new DeliveryEditFields(
                    entity.Deliverydate,
                    entity.InstallDate,
                    entity.DeliveryDepartmentId,
                    entity.DeliveryOu,
                    entity.DeliveryAddress,
                    entity.DeliveryPostalCode,
                    entity.DeliveryPostalAddress,
                    entity.DeliveryLocation,
                    entity.DeliveryInfo,
                    entity.DeliveryInfo2,
                    entity.DeliveryInfo3,
                    entity.DeliveryOuId);
        }

        private static GeneralEditFields CreateGeneralEditFields(Order entity)
        {
            return new GeneralEditFields(
                    entity.Id.ToString(CultureInfo.InvariantCulture),
                    entity.Customer.Name,
                    entity.User_Id,
                    entity.Domain_Id,
                    entity.OrderDate);
        }

        private static LogEditFields CreateLogEditFields(Order entity)
        {
            return new LogEditFields(entity.Logs.Select(l => 
                    new BusinessData.Models.Orders.Order.OrderEditFields.Log(l.Id, l.CreatedDate, new UserName(l.User.FirstName, l.User.SurName), l.LogNote)).ToList());
        }

        private static OrdererEditFields CreateOrdererEditFields(Order entity)
        {
            return new OrdererEditFields(
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

        private static OrderEditFields CreateOrderEditFields(Order entity)
        {
            return new OrderEditFields(
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

        private static OtherEditFields CreateOtherEditFields(Order entity)
        {
            return new OtherEditFields(
                    entity.Filename,
                    entity.CaseNumber,
                    entity.Info,
                    entity.OrderState_Id);
        }

        private static ProgramEditFields CreateProgramEditFields(Order entity)
        {
            return new ProgramEditFields(entity.Programs.Select(p => new OrderProgramModel(p.Id, p.Name)).ToList());
        }

        private static ReceiverEditFields CreateReceiverEditFields(Order entity)
        {
            return new ReceiverEditFields(
                    entity.ReceiverId,
                    entity.ReceiverName,
                    entity.ReceiverEMail,
                    entity.ReceiverPhone,
                    entity.ReceiverLocation,
                    entity.MarkOfGoods);
        }

        private static SupplierEditFields CreateSupplierEditFields(Order entity)
        {
            return new SupplierEditFields(
                    entity.SupplierOrderNumber,
                    entity.SupplierOrderDate,
                    entity.SupplierOrderInfo);
        }

        private static UserEditFields CreateUserEditFields(Order entity)
        {
            return new UserEditFields(
                    entity.UserId,
                    entity.UserFirstName,
                    entity.UserLastName);
        }
    }
}