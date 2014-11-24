namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts;
    using DH.Helpdesk.Services.Requests.Account;

    public class OrderAccountService : IOrderAccountService
    {
        private readonly UnitOfWorkFactory unitOfWorkFactory;

        public OrderAccountService(UnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public List<AccountOverview> GetOverviews(AccountFilter filter, OperationContext context)
        {
            using (var uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                IQueryable<Account> account =
                    accountRepository.GetAll()
                        .GetActivityTypeAccounts(filter.ActivityTypeId)
                        .GetAdministratorAccounts(filter.AdministratorTypeId)
                        .GetAccountsBySearchString(filter.SearchString)
                        .GetStateAccounts(filter.AccountState);

                return null;
            }
        }
    }
}
