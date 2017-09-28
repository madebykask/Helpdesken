using DH.Helpdesk.BusinessData.Models.Employee;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.Services.WebApi
{
    public interface IWebApiService
    {
        Task<bool> IsEmployeeManager(string employeeNumber);

        Task<EmployeeModel> GetEmployeeFor(string employeeNumber);
    }
}
