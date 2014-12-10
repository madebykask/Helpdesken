namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Requests.Account;

    public interface IOrderAccountService
    {
        List<AccountOverview> GetOverviews(AccountFilter filter, OperationContext context);

        AccountForEdit Get(int id);

        void Update(AccountForUpdate dto, OperationContext context);

        void Add(AccountForInsert dto, OperationContext context);

        List<ItemOverview> GetAccountActivivties();

        List<int> GetAccountActivivtieIds();

        List<ItemOverview> GetEmploymentTypes();

        List<AccountTypeOverview> GetAccountTypes(int activityTypeId);

        void Delete(int id);
    }
}
