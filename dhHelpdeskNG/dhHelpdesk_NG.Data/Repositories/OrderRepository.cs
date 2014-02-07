namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
    }

    public class OrderFieldSettingsRepository : RepositoryBase<OrderFieldSettings>, IOrderFieldSettingsRepository
    {
        public OrderFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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
        void ResetDefault(int exclude);
    }

    public class OrderTypeRepository : RepositoryBase<OrderType>, IOrderTypeRepository
    {
        public OrderTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (OrderType obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }
    }

    #endregion
}
