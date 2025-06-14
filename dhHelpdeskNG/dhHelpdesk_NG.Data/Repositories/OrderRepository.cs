using System.Data.Entity;

namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;

    #region ORDER

    public interface IOrderRepository : IRepository<Order>
    {
        // expandable ....
        Order GetOrder(int caseid);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Order GetOrder(int caseid)
        {
            Order dup = (from du in this.DataContext.Set<Order>()
                         join d in this.DataContext.Cases on du.CaseNumber equals d.CaseNumber
                         where d.Id == caseid
                         select du).FirstOrDefault();
            return dup;
        }
    }

    #endregion

    #region ORDEREMAILLOG

    public interface IOrderEMailLogRepository : IRepository<OrderEMailLog>
    {
    }

    public class OrderEMailLogRepository : RepositoryBase<OrderEMailLog>, IOrderEMailLogRepository
    {
        public OrderEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region ORDERFIELDSETTINGS

    public interface IOrderFieldSettingsRepository : IRepository<OrderFieldSettings>
    {
        IEnumerable<OrderFieldSettings> GetOrderFieldSettingsForMailTemplate(int customerId, int? orderTypeId);
    }

    public class OrderFieldSettingsRepository : RepositoryBase<OrderFieldSettings>, IOrderFieldSettingsRepository
    {
        public OrderFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


        public IEnumerable<OrderFieldSettings> GetOrderFieldSettingsForMailTemplate(int customerId, int? orderTypeId)
        {
           
                 var query = from ofs in this.DataContext.OrderFieldSettings
                             where ofs.Customer_Id == customerId && ofs.OrderType_Id == orderTypeId && ofs.Show == 1
                        select ofs;
         

            return query.OrderBy(x => x.Id);
        }

    }

    #endregion

    #region ORDERLOG

    public interface IOrderLogRepository : IRepository<OrderLog>
    {
    }

    public class OrderLogRepository : RepositoryBase<OrderLog>, IOrderLogRepository
    {
        public OrderLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region ORDERSTATE

    public interface IOrderStateRepository : IRepository<OrderState>
    {
    }

    public class OrderStateRepository : RepositoryBase<OrderState>, IOrderStateRepository
    {
        public OrderStateRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region ORDERTYPE

    public interface IOrderTypeRepository : IRepository<OrderType>
    {
        void ResetDefault(int exclude, int customerId);
        bool IsUserHasOrderTypes(int customerId, int userId);
    }

    public class OrderTypeRepository : RepositoryBase<OrderType>, IOrderTypeRepository
    {
        public OrderTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude, int customerId)
        {
            foreach (OrderType obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

        public bool IsUserHasOrderTypes(int customerId, int userId)
        {
            return Table.Include(x => x.Users)
                .Where(x => x.Customer_Id == customerId)
                .Where(t => t.IsActive == 1 && t.Parent_OrderType_Id == null)
                .Where(t => t.Users.Any(u => u.Id == userId))
                .Any();
        }
    }

    #endregion
}
