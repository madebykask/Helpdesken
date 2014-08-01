namespace DH.Helpdesk.NewSelfService.WebServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;    

    public interface IAMAPIService
    {
        
       Task<bool> IsEmployeeManager (string employeeNumber);

       Task<string> GetEmployeeFor(string managerEmployeeNum);

    }
}