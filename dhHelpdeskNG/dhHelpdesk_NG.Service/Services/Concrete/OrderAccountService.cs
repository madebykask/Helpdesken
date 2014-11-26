namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts;
    using DH.Helpdesk.Services.Requests.Account;

    using User = DH.Helpdesk.Domain.User;

    public class OrderAccountService : IOrderAccountService
    {
        private readonly UnitOfWorkFactory unitOfWorkFactory;

        public OrderAccountService(UnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public List<AccountOverview> GetOverviews(AccountFilter filter, OperationContext context)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                IQueryable<EmploymentType> epmloimentTypes = uof.GetRepository<EmploymentType>().GetAll();
                IQueryable<AccountType> accountTypes = uof.GetRepository<AccountType>().GetAll();

                List<AccountOverview> overviews =
                    accountRepository.GetAll()
                        .GetActivityTypeAccounts(filter.ActivityTypeId)
                        .GetAdministratorAccounts(filter.AdministratorTypeId)
                        .GetAccountsBySearchString(filter.SearchString)
                        .GetStateAccounts(filter.AccountState)
                        .MapToAccountOverview(epmloimentTypes, accountTypes);

                return overviews;
            }
        }

        public AccountForEdit Get(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();
                IQueryable<User> users = uof.GetRepository<User>().GetAll();

                AccountForEdit dto = accountRepository.GetAll().ExtractAccountDto(users);

                return dto;
            }
        }

        public void Update(AccountForUpdate dto)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {

            }
        }

        public void Add(AccountForInsert dto)
        {
            throw new global::System.NotImplementedException();
        }

        public void Delete(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                Account account = accountRepository.GetById(id);
                account.Programs.Clear();

                var accountEMailLogRepository = uof.GetRepository<AccountEMailLog>();
                accountEMailLogRepository.DeleteWhere(x => x.Account_Id == id);

                accountRepository.DeleteById(id);

                uof.Save();
            }
        }
    }
}
