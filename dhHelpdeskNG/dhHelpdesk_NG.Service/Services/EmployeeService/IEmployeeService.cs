using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.BusinessData.Models.WebApi;

namespace DH.Helpdesk.Services.Services.EmployeeService
{
    public interface IEmployeeService
    {
        EmployeeModel GetEmployee(int customeId, string employeeNumber, bool getFromApi = false, 
                                  WebApiCredentialModel webApiCredential = null);
    }
}
    