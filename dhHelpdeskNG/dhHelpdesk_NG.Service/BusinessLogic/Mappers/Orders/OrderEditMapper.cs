namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Shared.Data;

    public static class OrderEditMapper
    {
        public static OrderEditOptions MapToOrderEditOptions(
                                        IQueryable<OrderType> orderTypes,
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
            IQueryable<UnionItemOverview> query = orderTypes.Select(s => new UnionItemOverview { Id = s.Id, Name = s.Name, Type = "orderTypes" });

            if (settings.Other.Status.Show)
            {
                query = query.Union(statuses.Select(s => new UnionItemOverview { Id = s.Id, Name = s.Name, Type = "statuses" }));
            }

            if (settings.General.Administrator.Show)
            {
                query = query.Union(administrators.Select(a => new UnionItemOverview { Id = a.Id, Name = a.FirstName + " " + a.SurName, Type = "administrators" }));
            }

            if (settings.General.Domain.Show)
            {
                query = query.Union(domains.Select(d => new UnionItemOverview { Id = d.Id, Name = d.Name, Type = "domains" }));
            }

            if (settings.Orderer.Department.Show)
            {
                query = query.Union(departments.Select(d => new UnionItemOverview { Id = d.Id, Name = d.DepartmentName, Type = "departments" }));
            }

            if (settings.Orderer.Unit.Show)
            {
                query = query.Union(units.Select(u => new UnionItemOverview { Id = u.Id, Name = u.Name, Type = "units" }));
            }

            if (settings.Order.Property.Show)
            {
                query = query.Union(properties.Select(p => new UnionItemOverview { Id = p.Id, Name = p.OrderProperty, Type = "properties" }));
            }

            if (settings.Delivery.DeliveryDepartment.Show)
            {
                query = query.Union(deliveryDepartments.Select(d => new UnionItemOverview { Id = d.Id, Name = d.DepartmentName, Type = "deliveryDepartments" }));
            }

            if (settings.Delivery.DeliveryOuId.Show)
            {
                query = query.Union(deliveryDepartments.Select(u => new UnionItemOverview { Id = u.Id, Name = u.DepartmentName, Type = "deliveryOuIds" }));
            }

            var separator = Guid.NewGuid().ToString();

            if (settings.Log.Log.Show)
            {
                query = query.Union(administratorsWithEmails.Select(a => new UnionItemOverview { Id = a.Id, Name = a.FirstName + " " + a.SurName + separator + a.Email, Type = "administratorsWithEmails" }));                
            }

            var overviews = query
                .OrderBy(u => u.Type)
                .ThenBy(u => u.Name)
                .ToArray();

            return new OrderEditOptions(
                overviews.Where(o => o.Type == "orderTypes").First().Name,
                overviews.Where(o => o.Type == "statuses").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "administrators").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "domains").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "departments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "units").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "properties").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "deliveryDepartments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                overviews.Where(o => o.Type == "deliveryOuIds").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                emailGroups,
                workingGroupsWithEmails,
                overviews.Where(o => o.Type == "administratorsWithEmails").Select(a => { 
                            var values = a.Name.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                            return new ItemOverview(values[0], values.Length > 1 ? values[1].Split(';').First() : string.Empty);
                        }).ToList());
        }
    }
}