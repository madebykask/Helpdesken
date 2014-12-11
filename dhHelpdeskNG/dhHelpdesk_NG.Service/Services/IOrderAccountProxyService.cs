namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Services.Requests.Account;
    using DH.Helpdesk.Services.Response.Account;

    public interface IOrderAccountProxyService
    {
        List<AccountOverview> GetOverviews(AccountFilter filter, OperationContext context);

        List<ItemOverview> GetAccountActivities();

        AccountForEdit Get(int id);

        AccountOptionsResponse GetOptions(int activityType, OperationContext context);

        IdAndNameOverview GetAccountActivityItemOverview(int id);

        List<int> GetAccountActivivtieIds();

        void Update(AccountForUpdate dto, OperationContext context);

        int Add(AccountForInsert dto, OperationContext context);

        void Delete(int id);
    }
}