namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;

    public interface IAccountService
    {
        Account GetAccount(int caseid);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        
        public AccountService(
            IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        public Account GetAccount(int caseid)
        {
            return this._accountRepository.GetAccount(caseid);
        }
    }
}
