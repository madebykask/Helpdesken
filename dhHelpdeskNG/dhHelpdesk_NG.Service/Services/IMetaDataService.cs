
using DH.Helpdesk.BusinessData.Models.Employee;

namespace DH.Helpdesk.Services.Services
{
    public interface IMetaDataService
    {
        EmployeeModel GetManager(int customerId, string employeeNumber);
    }
}
