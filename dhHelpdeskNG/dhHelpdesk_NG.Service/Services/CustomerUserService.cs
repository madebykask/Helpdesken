namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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

        
    }
}
