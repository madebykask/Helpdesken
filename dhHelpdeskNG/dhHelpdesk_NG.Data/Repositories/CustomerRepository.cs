﻿namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;

    #region CUSTOMER

    public interface ICustomerRepository : IRepository<Customer>
    {
        string GetCustomerName(int customerId);

        IList<Customer> CustomersForUser(int userId);

        CustomerOverview FindById(int id);

        MailAddress GetCustomerEmail(int customerId);

        ItemOverview GetOverview(int customerId);
    }

    public sealed class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public string GetCustomerName(int customerId)
        {
            return this.DataContext.Customers.Where(c => c.Id == customerId).Select(c => c.Name).Single();
        }

        public IList<Customer> CustomersForUser(int userId)
        {
            var query = from customer in this.DataContext.Set<Customer>()
                         join customerUser in this.DataContext.Set<CustomerUser>().Where(o => o.User_Id == userId) on customer.Id equals customerUser.Customer_Id
                         orderby customer.Name
                         select customer;

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

        public MailAddress GetCustomerEmail(int customerId)
        {
            var email = this.DataContext.Customers
                    .Where(c => c.Id == customerId)
                    .Select(c => c.HelpdeskEmail)
                    .ToList()
                    .FirstOrDefault();

            if (string.IsNullOrEmpty(email) ||
                !EmailHelper.IsValid(email))
            {
                return null;
            }

            return new MailAddress(email);
        }

        public ItemOverview GetOverview(int customerId)
        {
            var entities = this.Table
                    .Where(c => c.Id == customerId)
                    .Select(c => new { Value = c.Id, c.Name })
                    .ToList();

            return entities
                    .Select(c => new ItemOverview(c.Name, c.Value.ToString(CultureInfo.InvariantCulture)))
                    .FirstOrDefault();                        
        }
    }

    #endregion

    #region CUSTOMERUSER

    public interface ICustomerUserRepository : IRepository<CustomerUser>
    {
        CustomerUser GetCustomerSettings(int customer, int user);
        IList<UserCustomer> GetCustomerUsersForStart(int userId);
        IList<CustomerUserList> GetCustomerUsersForStartFinal(int userId);
        IList<CustomerUser> GetCustomerUsersForCustomer(int customeId);
        IList<CustomerUser> GetCustomerUsersForUser(int userId);
        void UpdateUserSetting(UserCaseSetting newSetting);
    }

    public class CustomerUserRepository : RepositoryBase<CustomerUser>, ICustomerUserRepository
    {
        private readonly IEntityToBusinessModelMapper<Setting, CustomerSettings> customerSettingsMapper;

        public CustomerUserRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<Setting, CustomerSettings> customerSettingsMapper)
            : base(databaseFactory)
        {
            this.customerSettingsMapper = customerSettingsMapper;
        }

        public CustomerUser GetCustomerSettings(int customer, int user)
        {
            return (from customerUser in this.DataContext.Set<CustomerUser>()
                    join settings in this.DataContext.Set<Setting>() on customerUser.Customer_Id equals settings.Customer_Id
                    where customerUser.Customer_Id == customer && customerUser.User_Id == user
                    select customerUser)
                    .ToList()
                    .FirstOrDefault();
        }

        public IList<UserCustomer> GetCustomerUsersForStart(int userId)
        {
            var entities = (from cu in this.DataContext.CustomerUsers
                         join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                         join u in this.DataContext.Users on cu.User_Id equals u.Id
                         join s in this.DataContext.Settings on c.Id equals s.Customer_Id
                         where u.Id == userId
                         select new { UserId = u.Id, Customer = cu, Settings = s })
                         .ToList();

            return entities
                    .Select(uc => new UserCustomer(uc.UserId, uc.Customer, this.customerSettingsMapper.Map(uc.Settings)))
                    .ToList();
        }

        public IList<CustomerUser> GetCustomerUsersForCustomer(int customerId)
        {
            var query = from cu in this.DataContext.CustomerUsers
                         where cu.Customer_Id == customerId
                         select cu;

            return query.ToList();
        }

        public IList<CustomerUser> GetCustomerUsersForUser(int userId)
        {
            var query = from cu in this.DataContext.CustomerUsers
                         where cu.User_Id == userId
                         select cu;

            return query.ToList();
        }

        /// <summary>
        /// The get customer users for start final.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<CustomerUserList> GetCustomerUsersForStartFinal(int userId)
        {
            var query = from cu in this.DataContext.CustomerUsers
                        join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                        join u in this.DataContext.Users on cu.User_Id equals u.Id
                        join cs in this.DataContext.Cases on cu.Customer_Id equals cs.Customer_Id
                        where u.Id == userId
                        group cu by new { cu.Customer_Id, cu.Customer.Name, cs.FinishingDate, cs.StateSecondary_Id } into g
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

                userSettingEntity.CaseRegistrationDateStartFilter = newSetting.CaseRegistrationDateStartFilter;
                userSettingEntity.CaseRegistrationDateEndFilter = newSetting.CaseRegistrationDateEndFilter;
                userSettingEntity.CaseWatchDateStartFilter = newSetting.CaseWatchDateStartFilter;
                userSettingEntity.CaseWatchDateEndFilter = newSetting.CaseWatchDateEndFilter;
                userSettingEntity.CaseClosingDateStartFilter = newSetting.CaseClosingDateStartFilter;
                userSettingEntity.CaseClosingDateEndFilter = newSetting.CaseClosingDateEndFilter;
                userSettingEntity.CaseRegistrationDateFilterShow = newSetting.CaseRegistrationDateFilterShow;
                userSettingEntity.CaseWatchDateFilterShow = newSetting.CaseWatchDateFilterShow;
                userSettingEntity.CaseClosingDateFilterShow = newSetting.CaseClosingDateFilterShow;
                userSettingEntity.CaseClosingReasonFilter = newSetting.CaseClosingReasonFilter == string.Empty
                                                                            ? null
                                                                            : newSetting.CaseClosingReasonFilter;
            }
        }
    }

    #endregion
}
