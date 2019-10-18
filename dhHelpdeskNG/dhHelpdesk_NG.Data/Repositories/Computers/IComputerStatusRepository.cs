using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.Repositories.Computers
{
    public interface IComputerStatusRepository: INewRepository
    {
        IQueryable<ComputerStatus> GetByCustomer(int customerId);
    }
}
