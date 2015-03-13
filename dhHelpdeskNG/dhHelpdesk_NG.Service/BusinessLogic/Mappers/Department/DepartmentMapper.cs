namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Department
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;
    using DH.Helpdesk.Services.utils;

    public static class DepartmentMapper
    {
        public static ItemOverview[] MapToItemOverviews(this IQueryable<Department> query)
        {
            var entities = query.Select(d => new
                                    {
                                        d.Id,
                                        d.DepartmentName
                                    })
                                    .OrderBy(d => d.DepartmentName)
                                    .ToArray();

            var overviews = entities.Select(p => new ItemOverview(
                                            p.DepartmentName,
                                            p.Id.ToString(CultureInfo.InvariantCulture))).ToArray();

            return overviews;
        }

        public static List<Department> MapToUserDepartments(
                                            IQueryable<User> users,
                                            IQueryable<Customer> customers,
                                            IQueryable<Department> departments,
                                            IQueryable<DepartmentUser> userDepartments)
        {
            var entities = (from ud in userDepartments
                            join u in users on ud.User_Id equals u.Id
                            join d in departments on ud.Department_Id equals d.Id
                            join c in customers on d.Customer_Id equals c.Id
                            select d)
                            .OrderBy(d => d.DepartmentName)
                            .ToList();

            return entities;
        }

        public static List<ItemOverview> MapToUserDepartments(
                                        IQueryable<Region> regions,
                                        IQueryable<User> users,
                                        IQueryable<Department> departments,
                                        IQueryable<DepartmentUser> userDepartments,
                                        IQueryable<Customer> customers,
                                        int departmentFilterFormat,
                                        int regionId)
        {
            var entities = (from ud in userDepartments
                            join u in users on ud.User_Id equals u.Id
                            join d in departments on ud.Department_Id equals d.Id
                            join c in customers on d.Customer_Id equals c.Id
                            select d)
                            .IncludePath(d => d.Country)
                            .OrderBy(d => d.DepartmentName)
                            .ToList();

            if (!entities.Any())
            {
                entities = (from d in departments
                            join c in customers on d.Customer_Id equals c.Id
                            select d)
                            .IncludePath(d => d.Country)
                            .OrderBy(d => d.DepartmentName)
                            .ToList();
            }

            return entities.Where(d => d.Region_Id == regionId).Select(d => new ItemOverview(d.departmentDescription(departmentFilterFormat), d.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public static List<ItemOverview> MapToUserDepartments(
                                IQueryable<User> users,
                                IQueryable<Department> departments,
                                IQueryable<DepartmentUser> userDepartments,
                                IQueryable<Customer> customers,
                                int departmentFilterFormat)
        {
            var entities = (from ud in userDepartments
                            join u in users on ud.User_Id equals u.Id
                            join d in departments on ud.Department_Id equals d.Id
                            join c in customers on d.Customer_Id equals c.Id
                            select d)
                            .IncludePath(d => d.Country)                            
                            .OrderBy(d => d.DepartmentName)
                            .ToList();

            if (!entities.Any())
            {
                entities = (from d in departments
                            join c in customers on d.Customer_Id equals c.Id
                            select d)
                            .IncludePath(d => d.Country)
                            .OrderBy(d => d.DepartmentName)
                            .ToList();
            }

            return entities.Select(d => new ItemOverview(d.departmentDescription(departmentFilterFormat), d.Id.ToString(CultureInfo.InvariantCulture))).ToList();            
        }

        public static List<ItemOverview> MapToUserDepartments(
                                        IQueryable<Region> regions,
                                        IQueryable<Department> departments,
                                        IQueryable<Customer> customers,
                                        int departmentFilterFormat)
        {
            var entities = (from d in departments
                            join c in customers on d.Customer_Id equals c.Id
                            join r in regions on d.Region_Id equals r.Id
                            select d)
                            .IncludePath(d => d.Country)
                            .OrderBy(d => d.DepartmentName)
                            .ToList();

            return entities.Select(d => new ItemOverview(d.departmentDescription(departmentFilterFormat), d.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public static List<ItemOverview> MapToUserDepartments(
                                        IQueryable<Department> departments,
                                        IQueryable<Customer> customers,
                                        int departmentFilterFormat)
        {
            var entities = (from d in departments
                            join c in customers on d.Customer_Id equals c.Id
                            select d)
                            .IncludePath(d => d.Country)
                            .OrderBy(d => d.DepartmentName)
                            .ToList();

            return entities.Select(d => new ItemOverview(d.departmentDescription(departmentFilterFormat), d.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public static List<ItemOverview> MapToDepartmentUsers(
                                            IQueryable<User> users,
                                            IQueryable<Customer> customers,
                                            IQueryable<Department> departments,
                                            IQueryable<DepartmentUser> userDepartments)
        {
            var entities = (from ud in userDepartments
                            join u in users on ud.User_Id equals u.Id
                            join d in departments on ud.Department_Id equals d.Id
                            join c in customers on d.Customer_Id equals c.Id
                            select u)
                            .GetOrderedByName()
                            .ToList();

            return entities.Select(u => new ItemOverview(
                                        new UserName(u.FirstName, u.SurName).GetReversedFullName(),
                                        u.Id.ToString(CultureInfo.InvariantCulture)))
                                        .ToList();
        }
    }
}