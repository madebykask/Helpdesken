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
                                        o.Category,
                                        o.Customer,
                                        o.Object,
                                        o.LogAction
                                    })
                                    .ToArray();

            return entities.Select(o => new OperationLogOverview
                                    {
                                        ChangedDate = o.ChangedDate.ToLocalTime(),
                                        CreatedDate = o.CreatedDate,
                                        LogText = o.LogText,
                                        Category = new OperationLogCategoryOverview
                                        {
                                            OLCName = o.Category != null ? o.Category.OLCName : null
                                        },
                                        CustomerName = o.Customer.Name,
                                        Object = new OperationLogObjectOverview
                                        {
                                            Name = o.Object != null ? o.Object.Name : null,
                                            Status = o.Object.IsActive
                                        },
                                        LogAction = o.LogAction
                                    }).ToArray();
        }

        public static void MapToEntity(OperationLog model, OperationLog entity)
        {
            entity.Customer_Id = model.Customer_Id;
            entity.InformUsers = model.InformUsers;
            entity.OperationLogCategory_Id = model.OperationLogCategory_Id;
            entity.OperationObject_Id = model.OperationObject_Id;
            entity.Popup = model.Popup;
            entity.PublicInformation = model.PublicInformation;
            entity.ShowOnStartPage = model.ShowOnStartPage;
            entity.User_Id = model.User_Id;
            entity.WorkingTime = model.WorkingTime;
            entity.LogAction = model.LogAction ?? string.Empty;
            entity.LogText = model.LogText ?? string.Empty;
            entity.LogTextExternal = model.LogTextExternal ?? string.Empty;
            entity.ShowDate = model.ShowDate;
            entity.ShowUntilDate = model.ShowUntilDate;
            entity.CreatedDate = model.CreatedDate;
            entity.ChangedDate = model.ChangedDate;
        }
    }
}