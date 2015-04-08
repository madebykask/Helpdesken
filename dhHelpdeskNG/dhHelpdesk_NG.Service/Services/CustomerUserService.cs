namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICustomerUserService
    {
        IList<UserCustomer> GetCustomerUsersForHomeIndexPage(int userId);

        IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int id);

        IList<CustomerUser> GetCustomerUsersForCustomer(int customerId);

        IList<CustomerUser> GetCustomerUsersForUser(int userId);

        CustomerUser GetCustomerSettings(int customer, int user);

        UserCaseSetting GetUserCaseSettings(int customerId, int userId);

        void UpdateUserCaseSetting(UserCaseSetting newSetting);

        void SaveCustomerUser(CustomerUser customerUser, out IDictionary<string, string> errors);

        void SaveCustomerUserForCopy(CustomerUser customerUser, out IDictionary<string, string> errors);
    }

    public class CustomerUserService : ICustomerUserService
    {
        private readonly ICustomerUserRepository customerUserRepository;

        public CustomerUserService(
            ICustomerUserRepository customerUserRepository)
        {
            this.customerUserRepository = customerUserRepository;
        }

        public IList<UserCustomer> GetCustomerUsersForHomeIndexPage(int userId)
        {
            return this.customerUserRepository.GetCustomerUsersForStart(userId);
        }

        public IList<CustomerUser> GetCustomerUsersForCustomer(int customerId)
        {
            return this.customerUserRepository.GetCustomerUsersForCustomer(customerId);
        }

        public IList<CustomerUser> GetCustomerUsersForUser(int userId)
        {
            return this.customerUserRepository.GetCustomerUsersForUser(userId);
        }

        public IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int userId)
        {
            return this.customerUserRepository.GetCustomerUsersForStartFinal(userId);
        }

        public CustomerUser GetCustomerSettings(int customer, int user)
        {
            return this.customerUserRepository.GetCustomerSettings(customer, user);
        }

        public UserCaseSetting GetUserCaseSettings(int customerId, int userId)
        {
            var userSetting = this.customerUserRepository.GetCustomerSettings(customerId, userId);
            return new UserCaseSetting(
                  customerId,
                  userId,
                  (userSetting.CaseRegionFilter != null) ? userSetting.CaseRegionFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseUserFilter != null) ? userSetting.CaseUserFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseCaseTypeFilter != null ? userSetting.CaseCaseTypeFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseProductAreaFilter != null) ? userSetting.CaseProductAreaFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseWorkingGroupFilter != null) ? userSetting.CaseWorkingGroupFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseResponsibleFilter != null,
                  (userSetting.CasePerformerFilter != null) ? userSetting.CasePerformerFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CasePriorityFilter != null) ? userSetting.CasePriorityFilter.Replace(" ", string.Empty) : string.Empty,
                  userSetting.CaseStatusFilter != null,
                  (userSetting.CaseStateSecondaryFilter != null) ? userSetting.CaseStateSecondaryFilter.Replace(" ", string.Empty) : string.Empty,
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
                  userSetting.CaseInitiatorFilterShow);
        }

        public void UpdateUserCaseSetting(UserCaseSetting newSetting)
        {
            this.customerUserRepository.UpdateUserSetting(newSetting);
            this.customerUserRepository.Commit();
        }

        public void SaveCustomerUser(CustomerUser customerUser, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            this.customerUserRepository.Update(customerUser);
            //this.customerUserRepository.Add(customerUser);
            this.customerUserRepository.Commit();
        }

        public void SaveCustomerUserForCopy(CustomerUser customerUser, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            this.customerUserRepository.Add(customerUser);
            this.customerUserRepository.Commit();
        }
    }
}
