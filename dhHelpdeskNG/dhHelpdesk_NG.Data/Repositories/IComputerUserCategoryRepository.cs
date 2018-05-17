using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Computers;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models;

namespace DH.Helpdesk.Dal.Repositories
{
	public interface IComputerUserCategoryRepository : INewRepository
	{
	    bool CheckIfExtendedFormsExistForCategories(int customerId, List<int> ids);
        IList<ComputerUserCategoryOverview> GetAllByCustomerID(int customerID);
        ComputerUserCategory GetByID(int ID);
	}
}
