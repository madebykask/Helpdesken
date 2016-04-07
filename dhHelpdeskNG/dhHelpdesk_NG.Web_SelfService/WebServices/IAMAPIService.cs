namespace DH.Helpdesk.SelfService.WebServices
{

    using DH.Helpdesk.Common.Classes.ServiceAPI.AMAPI.Output;
    using System.Collections.Generic;
    using System.Threading.Tasks;    

    public interface IAMAPIService
    {
        
       Task<bool> IsEmployeeManager (string employeeNumber);

       Task<APIEmployee> GetEmployeeFor(string employeeNum);

    }
}