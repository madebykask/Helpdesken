using System.Collections.Generic;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public interface ICustomerUserService
    {
        IList<UserCustomer> GetCustomerUsersForHomeIndexPage(int userId);

        IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int id);

        IList<CustomerUser> GetCustomerUsersForCustomer(int customerId);

        IList<CustomerUser> GetCustomerUsersForUser(int userId);

        CustomerUser GetCustomerUserSettings(int customer, int user);
        Task<CustomerUser> GetCustomerUserSettingsAsync(int customer, int user);

        UserCaseSetting GetUserCaseSettings(int customerId, int userId);

        void UpdateUserCaseSetting(UserCaseSetting newSetting);

        void SaveCustomerUser(CustomerUser customerUser, out IDictionary<string, string> errors);

        void SaveCustomerUserForCopy(CustomerUser customerUser, out IDictionary<string, string> errors);

        IList<UserCustomerOverview> ListCustomersByUserCases(string userId, string employeeNumber, IList<string> employees, Customer customer);

        IList<UserCustomerOverview> GetUserCustomersWithCases(int userId);
    }

    public class CustomerUserService : ICustomerUserService
    {
        private readonly ICustomerUserRepository _customerUserRepository;

        public CustomerUserService(ICustomerUserRepository customerUserRepository)
        {
            _customerUserRepository = customerUserRepository;
        }

        public IList<UserCustomerOverview> ListCustomersByUserCases(string userId, string employeeNumber, IList<string> employees, Customer customer)
        {
            return _customerUserRepository.ListCustomersByUserCases(userId, employeeNumber, employees, customer);
        }

        public IList<UserCustomerOverview> GetUserCustomersWithCases(int userId)
        {
            return _customerUserRepository.GetUserCustomersWithCases(userId);
        }

        public IList<UserCustomer> GetCustomerUsersForHomeIndexPage(int userId)
        {
            return _customerUserRepository.GetCustomerUsersForStart(userId);
        }

        public IList<CustomerUser> GetCustomerUsersForCustomer(int customerId)
        {
            return _customerUserRepository.GetCustomerUsersForCustomer(customerId);
        }

        public IList<CustomerUser> GetCustomerUsersForUser(int userId)
        {
            return _customerUserRepository.GetCustomerUsersForUser(userId);
        }

        public IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int userId)
        {
            return _customerUserRepository.GetCustomerUsersForStartFinal(userId);
        }

        public CustomerUser GetCustomerUserSettings(int customer, int user)
        {
            return _customerUserRepository.GetCustomerSettings(customer, user);
        }

        public Task<CustomerUser> GetCustomerUserSettingsAsync(int customer, int user)
        {
            return _customerUserRepository.GetCustomerSettingsAsync(customer, user);
        }

        public UserCaseSetting GetUserCaseSettings(int customerId, int userId)
        {
            var userSetting = _customerUserRepository.GetCustomerSettings(customerId, userId);
            return new UserCaseSetting(
                  customerId,
                  userId,
                  (userSetting.CaseRegionFilter != null) ? userSetting.CaseRegionFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseDepartmentFilter != null) ? userSetting.CaseDepartmentFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseUserFilter != null) ? userSetting.CaseUserFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseCaseTypeFilter != null ? userSetting.CaseCaseTypeFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseProductAreaFilter != null) ? userSetting.CaseProductAreaFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseWorkingGroupFilter != null) ? userSetting.CaseWorkingGroupFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseResponsibleFilter != null,
                  (userSetting.CasePerformerFilter != null) ? userSetting.CasePerformerFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CasePriorityFilter != null) ? userSetting.CasePriorityFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseStatusFilter != null) ? userSetting.CaseStatusFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseStateSecondaryFilter != null) ? userSetting.CaseStateSecondaryFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseCategoryFilter != null) ? userSetting.CaseCategoryFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseRegistrationDateStartFilter,
                  userSetting.CaseRegistrationDateEndFilter,
                  userSetting.CaseWatchDateStartFilter,
                  userSetting.CaseWatchDateEndFilter,
                  userSetting.CaseClosingDateStartFilter,
                  userSetting.CaseClosingDateEndFilter,
                  userSetting.CaseRegistrationDateFilterShow,
                  userSetting.CaseWatchDateFilterShow,
                  userSetting.CaseClosingDateFilterShow,
                  (userSetting.CaseClosingReasonFilter != null) ? userSetting.CaseClosingReasonFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseInitiatorFilterShow,
                  (userSetting.CaseRemainingTimeFilter != null) ? userSetting.CaseRemainingTimeFilter.Replace(" ", string.Empty) : string.Empty);
        }

        public void UpdateUserCaseSetting(UserCaseSetting newSetting)
        {
            _customerUserRepository.UpdateUserSetting(newSetting);
            _customerUserRepository.Commit();
        }

        public void SaveCustomerUser(CustomerUser customerUser, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            _customerUserRepository.Update(customerUser);
            //this.customerUserRepository.Add(customerUser);
            _customerUserRepository.Commit();
        }

        public void SaveCustomerUserForCopy(CustomerUser customerUser, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            _customerUserRepository.Add(customerUser);
            _customerUserRepository.Commit();
        }
    }
}
