namespace DH.Helpdesk.Services.BusinessLogic.Mappers.OperationLogs
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.OperationLog.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Infrastructure.Security;
    using DH.Helpdesk.Domain;

    public static class OperationLogMapper
    {
        public static OperationLogOverview[] MapToOverviews(
                                    this IQueryable<OperationLog> query,
                                    IWorkContext workContext)
        {
            var entities = query.Select(o => new
                                    {
                                        o.Customer_Id,
                                        o.ChangedDate,
                                        o.CreatedDate,
                                        o.LogText,
                                        o.Category,
                                        o.ShowOnStartPage,
                                        o.Us,
                                        o.WGs
                                    })
                                    .OrderByDescending(p => p.CreatedDate)
                                    .ToArray();

            return entities.CheckAccess(workContext)
                            .Select(o => new OperationLogOverview
                                    {
                                        CustomerId = o.Customer_Id,
                                        ChangedDate = o.ChangedDate,
                                        CreatedDate = o.CreatedDate,
                                        LogText = o.LogText,
                                        Category = new OperationLogCategoryOverview
                                        {
                                            OLCName = o.Category != null ? o.Category.OLCName : null
                                        },
                                        ShowOnStartPage = o.ShowOnStartPage.ToBool()
                                    }).ToArray();
        }
    }
}