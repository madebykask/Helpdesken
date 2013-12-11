using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
            _accountRepository = accountRepository;
        }

        public Account GetAccount(int caseid)
        {
            return _accountRepository.GetAccount(caseid);
        }
    }
}
