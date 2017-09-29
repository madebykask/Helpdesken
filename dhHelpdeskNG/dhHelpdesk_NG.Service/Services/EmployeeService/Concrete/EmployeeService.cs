using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.Services.Services.WebApi;
using DH.Helpdesk.BusinessData.Models.WebApi;
using DH.Helpdesk.Services.Helpers;

namespace DH.Helpdesk.Services.Services.EmployeeService.Concrete
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMetaDataService _metaDataService;

        public EmployeeService(IMetaDataService metaDataService)
        {            
            _metaDataService = metaDataService;
        }

        public EmployeeModel GetEmployee(int customeId, string employeeNumber, bool getFromApi = false, WebApiCredentialModel webApiCredential = null)
        {
            return (getFromApi) ? 
                GetEmployeeFromApi(employeeNumber, webApiCredential) : 
                GetEmployeeFromMetaData(customeId, employeeNumber);
        }

        private EmployeeModel GetEmployeeFromApi(string employeeNumber, WebApiCredentialModel webApiCredential)
        {
            if (webApiCredential == null)
                return null;

            var webApiService = new WebApiService(webApiCredential);
            var employee = AsyncHelpers.RunSync<EmployeeModel>(() => webApiService.GetEmployeeFor(employeeNumber));
            return employee;
        }

        private EmployeeModel GetEmployeeFromMetaData(int customerId, string employeeNumber)
        {
            return _metaDataService.GetManager(customerId, employeeNumber);
        }
    }
}
