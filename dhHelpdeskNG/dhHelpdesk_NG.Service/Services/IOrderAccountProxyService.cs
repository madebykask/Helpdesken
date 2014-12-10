namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Services.Requests.Account;
    using DH.Helpdesk.Services.Response.Account;

    public interface IOrderAccountProxyService
    {
        AccountOverviewResponse GetOverviewResponse(AccountFilter filter, OperationContext context);

        AccountForEdit Get(int id);

        void Update(AccountForUpdate dto, OperationContext context);

        void Add(AccountForInsert dto, OperationContext context);

        void Delete(int id);
    }
}