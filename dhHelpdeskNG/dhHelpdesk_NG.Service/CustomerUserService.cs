using System.Collections.Generic;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using System.Linq;

namespace dhHelpdesk_NG.Service
{
    public interface ICustomerUserService
    {
        IList<CustomerUser> GetCustomerUsersForHomeIndexPage(int userId);
        IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int id);

        CustomerUser GetCustomerSettings(int customer, int user);
    }

    public class CustomerUserService : ICustomerUserService
    {
        private readonly ICustomerUserRepository _customerUserRepository;

        public CustomerUserService(
            ICustomerUserRepository customerUserRepository)
        {
            _customerUserRepository = customerUserRepository;
        }

        public IList<CustomerUser> GetCustomerUsersForHomeIndexPage(int userId)
        {
            return _customerUserRepository.GetCustomerUsersForStart(userId);
        }

        public IList<CustomerUserList> GetFinalListForCustomerUsersHomeIndexPage(int userId)
        {
            return _customerUserRepository.GetCustomerUsersForStartFinal(userId);
        }

        public CustomerUser GetCustomerSettings(int customer, int user)
        {
            return _customerUserRepository.GetCustomerSettings(customer, user);
        }

        
    }
}
