using System.Linq;
using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICustomerUserService
    {
        IList<CustomerUser> GetCustomerUsersForHomeIndexPage(int userId);
        IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int id);

        CustomerUser GetCustomerSettings(int customer, int user);

        UserCaseSetting GetUserCaseSettings(int customerId, int userId);

        void UpdateUserCaseSetting(UserCaseSetting newSetting);
    }

    public class CustomerUserService : ICustomerUserService
    {
        private readonly ICustomerUserRepository _customerUserRepository;

        public CustomerUserService(
            ICustomerUserRepository customerUserRepository)
        {
            this._customerUserRepository = customerUserRepository;
        }

        public IList<CustomerUser> GetCustomerUsersForHomeIndexPage(int userId)
        {
            return this._customerUserRepository.GetCustomerUsersForStart(userId);
        }

        public IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int userId)
        {
            return this._customerUserRepository.GetCustomerUsersForStartFinal(userId);
        }

        public CustomerUser GetCustomerSettings(int customer, int user)
        {
            return this._customerUserRepository.GetCustomerSettings(customer, user);
        }

        public UserCaseSetting GetUserCaseSettings(int customerId, int userId)
        {
            var userSetting = this._customerUserRepository.GetCustomerSettings(customerId, userId);
            return new UserCaseSetting
                (
                  customerId,
                  userId,
                  (userSetting.CaseRegionFilter!=null)?userSetting.CaseRegionFilter.Replace(" ", string.Empty): string.Empty,
                  (userSetting.CaseUserFilter != null)? userSetting.CaseUserFilter.Replace(" ", string.Empty): string.Empty,
                  (userSetting.CaseCaseTypeFilter !=null),
                  (userSetting.CaseProductAreaFilter != null) ? userSetting.CaseProductAreaFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseWorkingGroupFilter != null)? userSetting.CaseWorkingGroupFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseResponsibleFilter != null),
                  (userSetting.CasePerformerFilter != null)? userSetting.CasePerformerFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CasePriorityFilter != null)? userSetting.CasePriorityFilter.Replace(" ", string.Empty) : string.Empty,
                  (userSetting.CaseStatusFilter != null),
                  (userSetting.CaseStateSecondaryFilter != null)? userSetting.CaseStateSecondaryFilter.Replace(" ", string.Empty) : string.Empty
                );

        }

        public void UpdateUserCaseSetting(UserCaseSetting newSetting)
        {
            _customerUserRepository.UpdateUserSetting(newSetting);
            _customerUserRepository.Commit();
        }
    }
}
