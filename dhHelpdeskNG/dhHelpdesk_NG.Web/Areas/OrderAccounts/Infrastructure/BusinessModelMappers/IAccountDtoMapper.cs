namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;

    public interface IAccountDtoMapper
    {
        AccountForUpdate BuidForUpdate(AccountModel model, byte[] content, OperationContext operationContext);

        AccountForInsert BuidForInsert(AccountModel model, byte[] content, OperationContext operationContext);
    }
}