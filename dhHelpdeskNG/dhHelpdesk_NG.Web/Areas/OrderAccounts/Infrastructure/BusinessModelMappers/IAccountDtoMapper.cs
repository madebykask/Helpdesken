using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public interface IAccountDtoMapper
    {
        AccountForUpdate BuidForUpdate(AccountModel model, WebTemporaryFile file, OperationContext operationContext);

        AccountForInsert BuidForInsert(AccountModel model, WebTemporaryFile file, OperationContext operationContext);
    }
}