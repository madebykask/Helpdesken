namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerRepository : Repository, IComputerRepository
    {
        public ComputerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public ComputerResults GetComputerInventory(string computername, bool join)
        {
            return null;
        }

        public List<ComputerResults> Search(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var query =
                from c in this.DbContext.Computers
                join ct in this.DbContext.ComputerTypes on c.ComputerType_Id equals ct.Id into res
                from k in res.DefaultIfEmpty()
                where c.Customer_Id == customerId
                      && (
                          c.ComputerName.ToLower().Contains(s)
                          || c.Location.ToLower().Contains(s) || k.ComputerTypeDescription.ToLower().Contains(s))
                select new ComputerResults {
                    Id = c.Id,
                    ComputerName = c.ComputerName,
                    Location = c.Location, 
                    ComputerType = k.Name, 
                    ComputerTypeDescription = k.ComputerTypeDescription 
                };

            return query.OrderBy(x => x.ComputerName).ThenBy(x => x.Location).ToList();            
        }
    }
    
    #region COMPUTERTYPE

    #endregion
}
