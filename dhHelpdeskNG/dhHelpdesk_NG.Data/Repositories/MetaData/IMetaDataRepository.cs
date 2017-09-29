using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.MetaData;

namespace DH.Helpdesk.Dal.Repositories.MetaData
{
    public interface IMetaDataRepository: IRepository<MetaDataEntity>
    {
        EmployeeModel GetManager(int customerId, string employeeNumber);
    }
}
