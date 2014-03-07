

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region CUSTOMER

    public interface ICustomerRepository : IRepository<Customer>
    {
        IList<Customer> CustomersForUser(int userId);

        CustomerOverview FindById(int id);
    }

    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<Customer> CustomersForUser(int userId)
        {
            var query = (from customer in this.DataContext.Set<Customer>()
                         join customerUser in this.DataContext.Set<CustomerUser>().Where(o => o.User_Id == userId) on customer.Id equals customerUser.Customer_Id
                         orderby customer.Name
                         select customer);

            return query.ToList();
        }

        public CustomerOverview FindById(int id)
        {
            var customer =
                this.DataContext.Customers
                    .Where(x => x.Id == id)
                    .Select(x => new CustomerOverview { Id = x.Id }).FirstOrDefault();

            return customer;
        }
    }

    #endregion

    #region CUSTOMERUSER

    public interface ICustomerUserRepository : IRepository<CustomerUser>
    {
        CustomerUser GetCustomerSettings(int customer, int user);
        IList<CustomerUser> GetCustomerUsersForStart(int userId);
        IList<CustomerUserList> GetCustomerUsersForStartFinal(int userId);
        void UpdateUserSetting(UserCaseSetting newSetting);
    }

    public class CustomerUserRepository : RepositoryBase<CustomerUser>, ICustomerUserRepository
    {
        public CustomerUserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public CustomerUser GetCustomerSettings(int customer, int user)
        {
            return (from customerUser in this.DataContext.Set<CustomerUser>()
                    join settings in this.DataContext.Set<Setting>() on customerUser.Customer_Id equals settings.Customer_Id
                    where customerUser.Customer_Id == customer && customerUser.User_Id == user
                    select customerUser).FirstOrDefault();
        }

        public IList<CustomerUser> GetCustomerUsersForStart(int userId)
        {
            var query = (from cu in this.DataContext.CustomerUsers
                         join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                         join u in this.DataContext.Users on cu.User_Id equals u.Id
                         where u.Id == userId
                         select cu);

            return query.ToList();
        }

        public IList<CustomerUserList> GetCustomerUsersForStartFinal(int userId)
        {
            var query = from cu in this.GetCustomerUsersForStart(userId)
                        join c in this.DataContext.Cases on cu.Customer_Id equals c.Customer_Id
                        group cu by new { cu.Customer_Id, cu.Customer.Name, c.FinishingDate, c.StateSecondary_Id } into g
                        select new CustomerUserList
                        {
                            Customer_Id = g.Key.Customer_Id,
                            FinishingDate = g.Key.FinishingDate,
                            Name = g.Key.Name,
                            StateSecondary_Id = g.Key.StateSecondary_Id
                        };

            return query.OrderBy(x => x.Customer_Id).ToList();
        }

        public void UpdateUserSetting(UserCaseSetting newSetting)
        {
            var userSettingEntity = this.DataContext.CustomerUsers.Where(cu=> cu.Customer_Id == newSetting.CustomerId && cu.User_Id == newSetting.UserId).FirstOrDefault();

            if (userSettingEntity != null)
            {
                userSettingEntity.CaseRegionFilter = (newSetting.Region == string.Empty) ? null : newSetting.Region;
                userSettingEntity.CaseUserFilter = (newSetting.RegisteredBy == string.Empty)
                    ? null
                    : newSetting.RegisteredBy;
                userSettingEntity.CaseCaseTypeFilter = (newSetting.CaseType) ? "0" : null;
                userSettingEntity.CaseProductAreaFilter = (newSetting.ProductArea == string.Empty)
                    ? null
                    : newSetting.ProductArea;
                userSettingEntity.CaseWorkingGroupFilter = (newSetting.WorkingGroup == string.Empty)
                    ? null
                    : newSetting.WorkingGroup;
                userSettingEntity.CaseResponsibleFilter = (newSetting.Responsible) ? "0" : null;
                userSettingEntity.CasePerformerFilter = (newSetting.Administrators == string.Empty)
                    ? null
                    : newSetting.Administrators;
                userSettingEntity.CasePriorityFilter = (newSetting.Priority == string.Empty)
                    ? null
                    : newSetting.Priority;
                userSettingEntity.CaseStatusFilter = (newSetting.State) ? "0" : null;
                userSettingEntity.CaseStateSecondaryFilter = (newSetting.SubState == string.Empty)
                    ? null
                    : newSetting.SubState;
            }
        }
    }

    #endregion
}
