using DH.Helpdesk.Domain;
using DH.Helpdesk.NewSelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    public class DocumentController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICaseSolutionService _caseSolutionService;
 
  
        public DocumentController(IMasterDataService masterDataService,
                                  ICustomerService customerService,
                                  ICaseSolutionService caseSolutionService)
            : base(masterDataService)
        {
             this._customerService = customerService;
            this._caseSolutionService = caseSolutionService;
        }

        //
        // GET: /Document/

        public ActionResult Index(int customerId)
        {
            if (!CheckCustomerValidation(customerId))
                return null;

            ViewBag.PublicCustomerId = customerId;
            ViewBag.PublicCaseTemplate = GetCaseTemplates(customerId);

            return View();
        }

        private bool CheckCustomerValidation(int customerId)
        {
            var cu = _customerService.GetCustomer(customerId);
            return (cu == null) ? false : true;
        }

        private List<CaseSolution> GetCaseTemplates(int customerId, bool checkAuthentication = true)
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
