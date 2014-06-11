using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.NewSelfService.Infrastructure.Common
{
    public interface ICommonFunctions
    {
        List<CaseSolution> GetCaseTemplates(int customerId);
    }

    public class CommonFunctions:ICommonFunctions
    {                
        private readonly ICaseSolutionService _caseSolutionService;
 
        public CommonFunctions(ICaseSolutionService caseSolutionService)
        {
            _caseSolutionService = caseSolutionService;
        }

        public List<CaseSolution> GetCaseTemplates(int customerId)
        {            
            return _caseSolutionService.GetCaseSolutions(customerId).Where(t=> t.ShowInSelfService).ToList();           
        }
    }
}