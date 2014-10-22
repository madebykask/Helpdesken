namespace DH.Helpdesk.Services.BusinessLogic.Mappers.OperationLogs
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.OperationLog.Output;
    using DH.Helpdesk.Domain;

    public static class OperationLogMapper
    {
        public static OperationLogOverview[] MapToOverviews(this IQueryable<OperationLog> query)
        {
            var entities = query.Select(o => new
                                    {
                                        o.ChangedDate,
                                        o.CreatedDate,
                                        o.LogText,
                                        o.Category
                                    })
                                    .ToArray();

            return entities.Select(o => new OperationLogOverview
                                    {
                                        ChangedDate = o.ChangedDate,
                                        CreatedDate = o.CreatedDate,
                                        LogText = o.LogText,
                                        Category = new OperationLogCategoryOverview
                                        {
                                            OLCName = o.Category != null ? o.Category.OLCName : null
                                        }
                                    }).ToArray();
        }
    }
}