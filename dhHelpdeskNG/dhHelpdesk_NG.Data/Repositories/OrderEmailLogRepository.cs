namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;

    public class OrderEmailLogRepository : RepositoryBase<OrderEMailLog>, IOrderEmailLogRepository
    {
        public OrderEmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<OrderEMailLog> GetOrderEmailLogsByOrderId(int orderId)  
        {
            return (from l in this.DataContext.OrderEMailLogs
                    join h in this.DataContext.OrderHistories on l.OrderHistoryId equals h.Id
                    where h.Id == orderId
                    select l).ToList(); 
        }

        public List<OrderEMailLog> GetOrderEmailLogsByOrderHistoryId(int orderHistoryId)
        {
            return (from l in this.DataContext.OrderEMailLogs
                    where l.OrderHistoryId == orderHistoryId
                    select l).ToList(); 

        }

        public OrderEMailLog GetOrderEmailLogsByGuid(Guid Id)
        {
            return (from l in this.DataContext.OrderEMailLogs
                    where l.OrderEMailLogGUID == Id
                    select l).FirstOrDefault();

        }
    }
}