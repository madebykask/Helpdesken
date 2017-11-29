using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DH.Helpdesk.Dal.Infrastructure;

namespace DH.Helpdesk.Dal.Repositories
{
	public class ComputerUserCategoryRepository : Repository<ComputerUserCategory>, IComputerUserCategoryRepository
	{
		public ComputerUserCategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}

		public IList<ComputerUserCategory> GetAllByCustomerID(int customerID)
		{
			var all = this.DbSet.Where(o => o.CustomerID == customerID).Include(o => o.CaseSolution.ExtendedCaseForms).ToList();
			return all;
		}


		public ComputerUserCategory GetByID(int computerUserCategoryID)
		{
			var computerUserCategory = this.DbSet.Include(o => o.CaseSolution.ExtendedCaseForms)
				.Single(o => o.ID == computerUserCategoryID);
			return computerUserCategory;
		}
	}
}
