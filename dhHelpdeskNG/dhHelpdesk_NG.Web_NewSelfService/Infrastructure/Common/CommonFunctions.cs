using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.NewSelfService.Infrastructure.Common
{
    public class CommonFunctions
    {                
        private readonly ICaseSolutionService _caseSolutionService;
 
        public CommonFunctions(ICaseSolutionService caseSolutionService)
        {
            _caseSolutionService = caseSolutionService;
        }

        public List<CaseSolution> GetCaseTemplates(int customerId, bool checkAuthentication = true)
        {
            var ret = new List<CaseSolution>();             
            if (checkAuthentication)
            {
                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (identity == null)
                {
                    return ret;
                }
            }

            ret = _caseSolutionService.GetCaseSolutions(customerId).ToList();

            return ret;
        }
    }
}