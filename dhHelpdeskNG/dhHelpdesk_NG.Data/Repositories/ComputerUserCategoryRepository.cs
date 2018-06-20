using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Computers;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Infrastructure;

namespace DH.Helpdesk.Dal.Repositories
{
	public class ComputerUserCategoryRepository : Repository<ComputerUserCategory>, IComputerUserCategoryRepository
	{
		public ComputerUserCategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}

		public IList<ComputerUserCategoryOverview> GetAllByCustomerID(int customerID)
		{
			var all = 
                this.DbSet.Where(o => o.CustomerID == customerID)
                .Select(x => new ComputerUserCategoryOverview()
                    {
                        Id = x.ID,
                        Name = x.Name,
                        CustomerId =  customerID,
                        ComputerUsersCategoryGuid = x.ComputerUsersCategoryGuid,
                        IsReadOnly = x.IsReadOnly,
                        IsEmpty = x.IsEmpty
                    }).ToList();

			return all;
		}

		public ComputerUserCategory GetByID(int computerUserCategoryID)
		{
		    var computerUserCategory = 
                this.DbSet.Include(o => o.CaseSolution.ExtendedCaseForms)
                .Single(o => o.ID == computerUserCategoryID);

			return computerUserCategory;
		}
	    
	    public bool CheckIfExtendedFormsExistForCategories(int customerId, List<int> ids)
	    {
	        var query =
	            from cat in this.DbSet
                where ids.Contains(cat.ID) &&
	                  cat.CaseSolution.ExtendedCaseForms.Any()
	            select cat.ID;

	        var items = query.ToList();
	        return items.Count > 0;
	    }
    }
}
