using DH.Helpdesk.Common.Extensions.Integer;

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
		    FullOrderEditSettings settings,
            IQueryable<EmploymentType> employmentTypes,
            IQueryable<Region> regions,
            IQueryable<OrderFieldType> accountTypes)
	    {
		    IQueryable<UnionItemDependentOverview> query = null;

	        var allStatuses = statuses.Where(o => o.IsActive == 1).OrderBy(o => o.SortOrder).ToList();

		    if (settings.General.Administrator.Show)
		    {
                query = administrators.Select(a => new UnionItemDependentOverview{Id = a.Id, Name = a.FirstName + " " + a.SurName, Type = "administrators", DependentId = null});
		    }

		    if (settings.General.Domain.Show)
		    {
			    var union = domains.Select(d => new UnionItemDependentOverview {Id = d.Id, Name = d.Name, Type = "domains", DependentId = null});
			    query = query?.Union(union) ?? union;
		    }

            if (settings.User.EmploymentType.Show)
            {
                var union = employmentTypes.Select(d => new UnionItemDependentOverview { Id = d.Id, Name = d.Name, Type = "employmentTypes", DependentId = null });
                query = query?.Union(union) ?? union;
            }

            if (settings.Orderer.Department.Show ||
                settings.User.DepartmentId1.Show ||
                settings.User.DepartmentId2.Show)
            {
			    var union = departments.Select(d => new UnionItemDependentOverview {Id = d.Id, Name = d.DepartmentName, Type = "departments", DependentId = null});
			    query = query?.Union(union) ?? union;

                var regionsUnion = regions.Select(d => new UnionItemDependentOverview { Id = d.Id, Name = d.Name, Type = "regions", DependentId = null });
                query = query.Union(regionsUnion);

            }

            if (settings.Orderer.Unit.Show ||
                settings.User.UnitId.Show)
		    {
			    var union = units.Select(u => new UnionItemDependentOverview {Id = u.Id, Name = u.Name, Type = "units", DependentId = u.Department_Id});
			    query = query?.Union(union) ?? union;
		    }

		    var propertiesOverviews = new ItemOverview[0];
		    if (settings.Order.Property.Show)
		    {
			    propertiesOverviews = properties.MapToItemOverviews();
		    }

		    if (settings.Delivery.DeliveryDepartment.Show)
		    {
			    var union = deliveryDepartments.Select(d => new UnionItemDependentOverview {Id = d.Id, Name = d.DepartmentName, Type = "deliveryDepartments", DependentId = null});
			    query = query?.Union(union) ?? union;
		    }

		    if (settings.Delivery.DeliveryOuId.Show)
		    {
			    var union = deliveryOuIds.Select(u => new UnionItemDependentOverview {Id = u.Id, Name = u.Name, Type = "deliveryOuIds", DependentId = null});
			    query = query?.Union(union) ?? union;
		    }

		    var separator = Guid.NewGuid().ToString();

		    if (settings.Log.Log.Show)
		    {
			    var union = administratorsWithEmails.Select(a => new UnionItemDependentOverview {Id = a.Id, Name = a.FirstName + " " + a.SurName + separator + a.Email, Type = "administratorsWithEmails", DependentId = null});
			    query = query?.Union(union) ?? union;
		    }

            if (settings.AccountInfo.AccountTypeId.Show)
            {
                var union = accountTypes.Where(a => a.OrderField == OrderFieldTypes.AccountType)
                    .Select(a => new UnionItemDependentOverview { Id = a.Id, Name = a.Name, Type = "accountTypes", DependentId = null });
                query = query?.Union(union) ?? union;
            }

            if (settings.AccountInfo.AccountTypeId2.Show)
            {
                var union = accountTypes.Where(a => a.OrderField == OrderFieldTypes.AccountType2).
                    Select(a => new UnionItemDependentOverview { Id = a.Id, Name = a.Name, Type = "accountTypes2", DependentId = null });
                query = query?.Union(union) ?? union;
            }

            if (settings.AccountInfo.AccountTypeId3.Show)
            {
                var union = accountTypes.Where(a => a.OrderField == OrderFieldTypes.AccountType3).
                    Select(a => new UnionItemDependentOverview { Id = a.Id, Name = a.Name, Type = "accountTypes3", DependentId = null });
                query = query?.Union(union) ?? union;
            }

            if (settings.AccountInfo.AccountTypeId4.Show)
            {
                var union = accountTypes.Where(a => a.OrderField == OrderFieldTypes.AccountType4).
                    Select(a => new UnionItemDependentOverview { Id = a.Id, Name = a.Name, Type = "accountTypes4", DependentId = null });
                query = query?.Union(union) ?? union;
            }

            if (settings.AccountInfo.AccountTypeId5.Show)
            {
                var union = accountTypes.Where(a => a.OrderField == OrderFieldTypes.AccountType5).
                    Select(a => new UnionItemDependentOverview { Id = a.Id, Name = a.Name, Type = "accountTypes5", DependentId = null });
                query = query?.Union(union) ?? union;
            }


            var overviews = new UnionItemDependentOverview[0];

		    if (query != null)
		    {
			    overviews = query
				    .OrderBy(u => u.Type)
				    .ThenBy(u => u.Name)
				    .ToArray();
		    }

		    var editOptions = new OrderEditOptions(
			    orderTypeName,
                allStatuses.Select(o => new OrderStatusItem(o.Name, o.Id.ToString(CultureInfo.InvariantCulture), o.CreateCase.ToBool(), o.NotifyOrderer.ToBool(), o.NotifyReceiver.ToBool())).ToArray(),
			    overviews.Where(o => o.Type == "administrators").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
			    overviews.Where(o => o.Type == "domains").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
			    overviews.Where(o => o.Type == "departments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
			    overviews.Where(o => o.Type == "units").Select(o => new ItemDependentOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture),
								o.DependentId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty)).ToArray(),
			    propertiesOverviews,
			    overviews.Where(o => o.Type == "deliveryDepartments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
			    overviews.Where(o => o.Type == "deliveryOuIds").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
			    emailGroups,
			    workingGroupsWithEmails,
			    overviews.Where(o => o.Type == "administratorsWithEmails").Select(a =>
			    {
				    var values = a.Name.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
				    return new ItemOverview(values[0], values.Length > 1 ? values[1].Split(';').First() : string.Empty);
			    }).ToList(),
                overviews.Where(o => o.Type == "employmentTypes").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "regions").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "accountTypes").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "accountTypes2").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "accountTypes3").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "accountTypes4").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "accountTypes5").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
	        return editOptions;
	    }

	    public static FullOrderEditFields MapToFullOrderEditFields(this IQueryable<Order> query)
        {
            var entity = query.Single();

            return MapToFullOrderEditFields(entity);
        }

        public static FullOrderEditFields MapToFullOrderEditFields(Order entity)
        {
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
                    CreateUserEditFields(entity),
                    CreateAccountInfoEditFields(entity),
                    CreateContactEditFields(entity));
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
                    entity.DeliveryOuId,
                    entity.DeliveryName,
                    entity.DeliveryPhone);
        }

        private static GeneralEditFields CreateGeneralEditFields(Order entity)
        {
            return new GeneralEditFields(
                    entity.Id,
                    entity.Customer.Name,
                    entity.User_Id,
                    entity.Domain_Id,
                    entity.OrderDate,
                    entity.OrderState_Id);
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
                    entity.Info);
            //, entity.OrderState_Id);
        }

        private static ProgramEditFields CreateProgramEditFields(Order entity)
        {
            return new ProgramEditFields(
                entity.Programs.Select(p => new OrderProgramModel(p.Id, p.Name)).ToList(),
                entity.InfoProduct);
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
                    entity.EmploymentType_Id,
                    entity.UserDepartment_Id,
                    entity.UserDepartment_Id2,
                    entity.UserDepartment1?.Region_Id);
        }

        private static AccountInfoEditFields CreateAccountInfoEditFields(Order entity)
        {
            return new AccountInfoEditFields(
                    entity.AccountStartDate,
                    entity.AccountEndDate,
                    entity.EMailType,
                    entity.HomeDirectory,
                    entity.Profile,
                    entity.InventoryNumber,
                    entity.AccountInfo,
                    entity.OrderFieldType_Id,
                    string.IsNullOrEmpty(entity.OrderFieldType2) ?
                        new List<int>() :
                        entity.OrderFieldType2.Split(',').Select(int.Parse).ToList(),
                    entity.OrderFieldType3_Id,
                    entity.OrderFieldType4_Id,
                    entity.OrderFieldType5_Id
                );
        }

        private static ContactEditFields CreateContactEditFields(Order entity)
        {
            return new ContactEditFields(
                    entity.ContactId,
                    entity.ContactName,
                    entity.ContactPhone,
                    entity.ContactEMail);
        }
    }

}