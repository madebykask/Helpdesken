namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Users
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;

    public static class UsersMapper
    {
        public static List<User> MapToCustomerUsers(
                                    IQueryable<Customer> customers,
                                    IQueryable<User> users,
                                    IQueryable<CustomerUser> cusstomerUsers)
        {
            var entities = (from cu in cusstomerUsers
                            join c in customers on cu.Customer_Id equals c.Id
                            join u in users on cu.User_Id equals u.Id
                            select u)
                            .GetOrderedByName()
                            .ToList();

            return entities;
        } 
    }
}