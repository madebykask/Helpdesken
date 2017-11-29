using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories
{
	public interface IComputerUserCategoryRepository : INewRepository
	{
		IList<ComputerUserCategory> GetAllByCustomerID(int customerID);
		ComputerUserCategory GetByID(int ID);
	}
}
