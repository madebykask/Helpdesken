using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    public class ComputerStatusRepository: Repository<ComputerStatus>, IComputerStatusRepository
    {
        public ComputerStatusRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        public IQueryable<ComputerStatus> GetByCustomer(int customerId)
        {
            return DbSet.AsNoTracking().Where(cs => cs.Customer_Id == customerId);
        }

        public int GetLastId()
        {
            return DbSet.AsNoTracking().Max(cs => cs.Id);
        }

        public void Add(ComputerStatus entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            DbSet.Add(entity);
        }
    }
}
