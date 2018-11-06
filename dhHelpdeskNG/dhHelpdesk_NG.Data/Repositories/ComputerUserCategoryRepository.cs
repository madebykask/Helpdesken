using DH.Helpdesk.Domain.Computers;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Infrastructure;

namespace DH.Helpdesk.Dal.Repositories
{
	public class ComputerUserCategoryRepository : RepositoryBase<ComputerUserCategory>, IComputerUserCategoryRepository
	{
		public ComputerUserCategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}

		public IList<ComputerUserCategoryOverview> GetAllByCustomerID(int customerID)
		{
			var all = 
                this.Table.Where(o => o.CustomerID == customerID)
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

	    public ComputerUserCategoryOverview GetEmptyCategoryOverview(int customerId)
	    {
	        var emptyCategory =
	            this.Table.Where(o => o.CustomerID == customerId && o.IsEmpty)
	                .Select(x => new ComputerUserCategoryOverview()
	                {
	                    Id = x.ID,
	                    Name = x.Name,
	                    IsEmpty = x.IsEmpty
	                })
	                .FirstOrDefault();

            return emptyCategory;
	    }

        public ComputerUserCategory GetByID(int computerUserCategoryID)
		{
		    var computerUserCategory = 
                this.Table.Include(o => o.CaseSolution.ExtendedCaseForms)
                .Single(o => o.ID == computerUserCategoryID);

			return computerUserCategory;
		}
	    
	    public bool CheckIfExtendedFormsExistForCategories(int customerId, List<int> ids)
	    {
	        //var query = Table.Where(cat => ids.Contains(cat.ID) && cat.CaseSolution.ExtendedCaseForms.Any()).Select(cat => cat.ID);
            var query = 
	            from cs in DataContext.ComputerUserCategories.Where(c => ids.Contains(c.ID)).Select(c => c.CaseSolution)
	            from exCaseSec in cs.CaseSectionsExtendedCaseForm
	            where cs.Customer_Id == customerId
                select exCaseSec.ExtendedCaseFormID;

	        var items = query.ToList();
	        return items.Count > 0;
	    }
    }
}
