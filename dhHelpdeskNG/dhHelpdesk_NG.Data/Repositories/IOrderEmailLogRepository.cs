namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;

    public interface IOrderEmailLogRepository : IRepository<OrderEMailLog>
    {
        List<OrderEMailLog> GetOrderEmailLogsByOrderId(int orderId);

        List<OrderEMailLog> GetOrderEmailLogsByOrderHistoryId(int orderHistoryId);

        OrderEMailLog GetOrderEmailLogsByGuid(Guid Id);
        
    }
}