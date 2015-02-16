namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Department
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;

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
    }
}