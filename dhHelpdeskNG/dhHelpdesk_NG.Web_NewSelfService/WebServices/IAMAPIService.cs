namespace DH.Helpdesk.NewSelfService.WebServices
{
    using DH.Helpdesk.BusinessData.Models.ServiceAPI.AMAPI.Output;
    using System.Collections.Generic;
    using System.Threading.Tasks;    

    public interface IAMAPIService
    {
        
       Task<bool> IsEmployeeManager (string employeeNumber);

       Task<APIEmployee> GetEmployeeFor(string employeeNum);

    }
}