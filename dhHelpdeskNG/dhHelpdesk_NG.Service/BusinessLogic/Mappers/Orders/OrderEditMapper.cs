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
            var overviews = new UnionItemOverview[0];

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

            if (settings.Order.Property.Show)
            {
                var union = properties.Select(p => new UnionItemOverview { Id = p.Id, Name = p.OrderProperty, Type = "properties" });
                query = query == null ? union : query.Union(union);
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

            if (query != null)
            {
                overviews = query
                    .OrderBy(u => u.Type)
                    .ThenBy(u => u.Name)
                    .ToArray();
            }

            return new OrderEditOptions(
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
                overviews.Where(o => o.Type == "administratorsWithEmails").Select(
                    a =>
                        {
                            var values = a.Name.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                            return new ItemOverview(values[0], values.Length > 1 ? values[1].Split(';').First() : string.Empty);
                        }).ToList());
        }
    }
}