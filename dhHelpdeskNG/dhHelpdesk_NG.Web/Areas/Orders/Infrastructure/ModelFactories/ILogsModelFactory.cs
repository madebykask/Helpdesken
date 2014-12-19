namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public interface ILogsModelFactory
    {
        LogsModel Create(int orderId, Subtopic area, List<Log> logs, OrderEditOptions options);
    }
}