using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region COMPUTER

    public interface IComputerRepository : IRepository<Computer>
    {
        ComputerResults GetComputerInventory(string computername, bool join);
        IList<ComputerResults> Search(int customerId, string searchFor);
    }

    public class ComputerRepository : RepositoryBase<Computer>, IComputerRepository
    {
        public ComputerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public ComputerResults GetComputerInventory(string computername, bool join)
        {
            return null;
        }

        public IList<ComputerResults> Search(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var query =
                from c in DataContext.Computers
                join ct in DataContext.ComputerTypes on c.ComputerType_Id equals ct.Id into res
                from k in res.DefaultIfEmpty()
                where c.Customer_Id == customerId
                      && (
                          c.ComputerName.ToLower().Contains(s)
                          || c.Location.ToLower().Contains(s)
                          || k.ComputerTypeDescription.ToLower().Contains(s)
                          )
                select new ComputerResults
                {
                    Id = c.Id,
                    ComputerName = c.ComputerName,
                    Location = c.Location, 
                    ComputerType = k.Name, 
                    ComputerTypeDescription = k.ComputerTypeDescription 
                };

            return query.OrderBy(x => x.ComputerName).ThenBy(x => x.Location).ToList();            
        }
    }

    #endregion

    #region COMPUTERFIELDSETTING

    public interface IComputerFieldSettingsRepository : IRepository<ComputerFieldSettings>
    {
    }

    public class ComputerFieldSettingsRepository : RepositoryBase<ComputerFieldSettings>, IComputerFieldSettingsRepository
    {
        public ComputerFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region COMPUTERHISTORY

    public interface IComputerHistoryRepository : IRepository<ComputerHistory>
    {
    }

    public class ComputerHistoryRepository : RepositoryBase<ComputerHistory>, IComputerHistoryRepository
    {
        public ComputerHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region COMPUTERLOG

    public interface IComputerLogRepository : IRepository<ComputerLog>
    {
    }

    public class ComputerLogRepository : RepositoryBase<ComputerLog>, IComputerLogRepository
    {
        public ComputerLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region COMPUTERMODEL

    public interface IComputerModelRepository : IRepository<ComputerModel>
    {
    }

    public class ComputerModelRepository : RepositoryBase<ComputerModel>, IComputerModelRepository
    {
        public ComputerModelRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region COMPUTERTYPE

    public interface IComputerTypeRepository : IRepository<ComputerType>
    {
    }

    public class ComputerTypeRepository : RepositoryBase<ComputerType>, IComputerTypeRepository
    {
        public ComputerTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
