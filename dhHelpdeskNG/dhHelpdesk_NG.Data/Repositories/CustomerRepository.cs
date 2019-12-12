using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories
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
using System;

    #region CUSTOMER

    public interface ICustomerRepository : IRepository<Customer>
    {
        string GetCustomerName(int customerId);

        IQueryable<Customer> CustomersForUser(int userId);
        IQueryable<Customer> GetAllowCaseMoveCustomers();

        CustomerOverview FindById(int id);

        MailAddress GetCustomerEmail(int customerId);

        ItemOverview GetOverview(int customerId);

        int? GetCustomerIdByEMailGUID(Guid GUID);

        int GetCustomerLanguage(int customerid);

        CaseDefaultsInfo GetCustomerDefaults(int customerId, bool isSelfService = false);

        int GetCustomerByCaseId(int caseId);
    }

    public sealed class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


        public int GetCustomerByCaseId(int caseId)
        {
            return DataContext.Cases.Where(x => x.Id == caseId).Select(x => x.Customer_Id).Single();
        }

        public string GetCustomerName(int customerId)
        {
            return this.DataContext.Customers.Where(c => c.Id == customerId).Select(c => c.Name).Single();
        }

        public int GetCustomerLanguage(int customerId)
        {
            return this.DataContext.Customers.Where(c => c.Id == customerId).Select(c => c.Language_Id).Single();
        }

        public int? GetCustomerIdByEMailGUID(Guid GUID)// TODO: optimise to 1 query
        {
            int? ret = null;
            var caseHistoryId = DataContext.EmailLogs.Where(e => e.EmailLogGUID == GUID && e.CaseHistory_Id != null)
                                                     .Select(e => e.CaseHistory_Id)
                                                     .FirstOrDefault();
            if (caseHistoryId != null)
            {
                var caseId = DataContext.CaseHistories.Where(h => h.Id == caseHistoryId).Select(h => h.Case_Id).FirstOrDefault();
                ret = DataContext.Cases.Where(c => c.Id == caseId).Select(c=> c.Customer_Id).FirstOrDefault();
            }

            return ret;
        }
        
        public IQueryable<Customer> CustomersForUser(int userId)
        {
            var query = 
                from cus in this.Table
                where cus.Users.Any(u => u.Id == userId)
                orderby cus.Name
                select cus;

            return query;
        }

        public IQueryable<Customer> GetAllowCaseMoveCustomers()
        {
            var query =
                from cus in this.Table
                from cusSettings in DataContext.Settings.Where(x => x.Customer_Id == cus.Id).DefaultIfEmpty()
                where cusSettings.AllowMoveCaseToAnyCustomer
                orderby cus.Name
                select cus;

            return query;
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
            var customerOverview = Table
                    .Where(c => c.Id == customerId)
                    .Select(c => new ItemOverview() { Value = c.Id.ToString(), Name = c.Name })
                    .FirstOrDefault();

            return customerOverview;                        
        }

        public CaseDefaultsInfo GetCustomerDefaults(int customerId, bool isSelfService = false)
        {
            var caseTypeCondition = 
                isSelfService 
                    ? (Expression<Func<CaseType, bool>>)(x => x.Customer_Id == customerId && x.IsActive == 1 && x.IsDefault == 1 && x.ShowOnExternalPage == 1)
                    : (x => x.Customer_Id == customerId && x.IsDefault == 1 && x.IsActive == 1 && x.Selectable == 1);

            var res =
                (from customer in this.Table
                 where customer.Id == customerId
                 select new 
                 {
                     RegionId = this.DataContext.Regions.Where(x => x.Customer_Id == customer.Id && x.IsActive == 1 && x.IsDefault == 1).Select(x => x.Id).FirstOrDefault(),
                     CaseTypeId = this.DataContext.CaseTypes.Where(caseTypeCondition).Select(x => x.Id).FirstOrDefault(),
                     SupplierId = this.DataContext.Suppliers.Where(x => x.Customer_Id == customer.Id && x.IsActive == 1 && x.IsDefault == 1).Select(x => x.Id).FirstOrDefault(),
                     PriorityId = this.DataContext.Priorities.Where(x => x.Customer_Id == customer.Id && x.IsActive == 1 && x.IsDefault == 1).Select(x => x.Id).FirstOrDefault(),
                     StatusId = this.DataContext.Statuses.Where(x => x.Customer_Id == customer.Id && x.IsActive == 1 && x.IsDefault == 1).Select(x => x.Id).FirstOrDefault(),
                 }).FirstOrDefault();

            //its important to return null for nullable values
            return res != null
                ? new CaseDefaultsInfo
                {
                    RegionId = res.RegionId == 0 ? null : (int?) res.RegionId,
                    CaseTypeId = res.CaseTypeId,
                    SupplierId = res.SupplierId == 0 ? null : (int?) res.SupplierId,
                    PriorityId = res.PriorityId == 0 ? null : (int?) res.PriorityId,
                    StatusId = res.StatusId == 0 ? null : (int?) res.StatusId
                } : new CaseDefaultsInfo();
        }
    }

    #endregion

    #region CUSTOMERUSER

    public interface ICustomerUserRepository : IRepository<CustomerUser>
    {
        CustomerUser GetCustomerSettings(int customer, int user);
        Task<CustomerUser> GetCustomerSettingsAsync(int customerId, int userId);
        IList<UserCustomer> GetCustomerUsersForStart(int userId);
        IList<CustomerUserList> GetCustomerUsersForStartFinal(int userId);
        IList<CustomerUser> GetCustomerUsersForCustomer(int customeId);
        IList<CustomerUser> GetCustomerUsersForUser(int userId);
        IList<CustomerUser> GetCustomerUsersForToCopy(int userId);
        void UpdateUserSetting(UserCaseSetting newSetting);
        bool IsCustomerUser(int customerId, int userId);
        bool CheckUserCasePermissions(int userId, int caseId, Expression<Func<Case, bool>> casePermissionsFilter = null);
        CustomerUser GetCustomerSettingsByCustomer(int customerId);

        IList<UserCustomerOverview> ListCustomersByUserCases(string userId, string employeeNumber, IList<string> employees, Customer customer);
        IList<UserCustomerOverview> GetUserCustomersWithCases(int userId);

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

        public CustomerUser GetCustomerSettings(int customerId, int userId)
        {
            return GetCustomerSettingsQuery(customerId, userId).FirstOrDefault();
        }

        public Task<CustomerUser> GetCustomerSettingsAsync(int customerId, int userId)
        {
            return GetCustomerSettingsQuery(customerId, userId).FirstOrDefaultAsync();
        }

        public IList<UserCustomerOverview> ListCustomersByUserCases(string userId, string employeeNumber, IList<string> employees, Customer customer)
        {
            var findByInitiator = customer.MyCasesInitiator;
            var findByRegistrator = customer.MyCasesRegistrator;
            var findByGroupUsers = customer.MyCasesUserGroup;

            var queryable = (from _case in DataContext.Cases
                             where  (findByInitiator || findByRegistrator || findByGroupUsers) &&
                                    (
                                        (findByRegistrator && (userId != null && userId.Trim() != "") && _case.RegUserId == userId) ||
                                        (
                                            findByInitiator &&
                                            (
                                                ((userId != null && userId.Trim() != "") && _case.ReportedBy == userId) ||
                                                ((employeeNumber != null && employeeNumber.Trim() != "") && _case.ReportedBy == employeeNumber)
                                            )

                                        ) ||
                                        (
                                            findByGroupUsers && 
                                            (
                                                ((_case.ReportedBy == null || _case.ReportedBy.Trim() == "") && _case.RegUserId == userId) ||
                                                employees.Contains(_case.ReportedBy)
                                            )
                                        )
                                        
                                    ) &&
                                    _case.Deleted == 0
                             group _case by new { CustomerId = _case.Customer_Id, CustomerName = _case.Customer.Name } into grouppedCases
                             select new UserCustomerOverview
                             {
                                 CustomerId = grouppedCases.Key.CustomerId,
                                 CustomerName = grouppedCases.Key.CustomerName,
                                 CasesCount = grouppedCases.Count()
                             }).OrderByDescending(x => x.CasesCount);

            return queryable.ToList();
        }

        public IList<UserCustomerOverview> GetUserCustomersWithCases(int userId)
        {
            var entities = (from cu in DataContext.CustomerUsers
                            join c in DataContext.Customers on cu.Customer_Id equals c.Id
                            where cu.User_Id == userId 
                            select new UserCustomerOverview
                            {
                                UserId = cu.User_Id,
                                CustomerId = c.Id,
                                CustomerName = c.Name,
                                CasesCount = DataContext.Cases.Where(x => x.Customer_Id == c.Id).Count()
                            })
                         .ToList();

            return entities;
        }

        public IList<UserCustomer> GetCustomerUsersForStart(int userId)
        {
            var entities = (from cu in this.DataContext.CustomerUsers
                            join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                            join u in this.DataContext.Users on cu.User_Id equals u.Id
                            join s in this.DataContext.Settings on c.Id equals s.Customer_Id
                            where u.Id == userId && cu.ShowOnStartPage != 0
                            select new
                            {
                                UserId = u.Id,
                                Customer = c,
                                CustomerUser = cu,
                                Settings = s
                            })
                         .ToList();

            return entities
                    .Select(uc => new UserCustomer(uc.UserId, uc.CustomerUser, this.customerSettingsMapper.Map(uc.Settings)))
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

        public IList<CustomerUser> GetCustomerUsersForToCopy(int userId)
        {
            var res = Table.AsNoTracking().Where(x => x.User_Id == userId).ToList();
            return res;
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
            var userSettingEntity = this.DataContext.CustomerUsers.Where(cu => cu.Customer_Id == newSetting.CustomerId && cu.User_Id == newSetting.UserId).FirstOrDefault();

            if (userSettingEntity != null)
            {
                userSettingEntity.CaseRegionFilter = (newSetting.Region == string.Empty) ? null : newSetting.Region;
                userSettingEntity.CaseDepartmentFilter = (newSetting.Departments == string.Empty)
                                                             ? null
                                                             : newSetting.Departments;
                userSettingEntity.CaseUserFilter = (newSetting.RegisteredBy == string.Empty)
                    ? null
                    : newSetting.RegisteredBy;
                userSettingEntity.CaseCaseTypeFilter = (newSetting.CaseType == string.Empty)
                    ? null
                    : newSetting.CaseType;
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
                //userSettingEntity.CaseStatusFilter = (newSetting.State) ? "0" : null;
                userSettingEntity.CaseStatusFilter = (newSetting.State == string.Empty)
                    ? null
                    : newSetting.State;
                userSettingEntity.CaseStateSecondaryFilter = (newSetting.SubState == string.Empty)
                    ? null
                    : newSetting.SubState;
                userSettingEntity.CaseCategoryFilter = (newSetting.Category == string.Empty)
                    ? null
                    : newSetting.Category;

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
                userSettingEntity.CaseInitiatorFilterShow = newSetting.CaseInitiatorFilterShow;

                userSettingEntity.CaseRemainingTimeFilter = (newSetting.CaseRemainingTime == string.Empty)
                                                             ? null
                                                             : newSetting.CaseRemainingTime;
            }
        }

        public bool IsCustomerUser(int customerId, int userId)
        {
            return DataContext.CustomerUsers.Any(cu => cu.Customer_Id == customerId && cu.User_Id == userId);
        }

        public bool CheckUserCasePermissions(int userId, int caseId, Expression<Func<Case, bool>> casePermissionsFilter = null)
        {
            IQueryable<Case> query =
                        from _case in DataContext.Set<Case>()
                        from user in _case.Customer.Users
                        where _case.Id == caseId &&
                        user.Id == userId
                        select _case;

            if (casePermissionsFilter != null)
            {
                query = query.Where(casePermissionsFilter);
            }
                        
            return query.Any();
        }

        public CustomerUser GetCustomerSettingsByCustomer(int customerId)
        {
            return Table
                    .Include(x => x.User)
                    .Where(cu => cu.Customer_Id == customerId).FirstOrDefault();
        }

        private IQueryable<CustomerUser> GetCustomerSettingsQuery(int customerId, int userId)
        {
            return Table
                .Include(x => x.User)
                .Where(cu => cu.Customer_Id == customerId && cu.User_Id == userId);
        }
    }

    #endregion
}
