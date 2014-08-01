using DH.Helpdesk.NewSelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.NewSelfService.WebServices;
    using DH.Helpdesk.NewSelfService.WebServices.Common;
    using System.Web.Script.Serialization;

    public class CoWorkersController : BaseController
    {        
        private readonly ICustomerService _customerService;
        private readonly ICaseSolutionService _caseSolutionService;

        public CoWorkersController(IMasterDataService masterDataService,
                                   ICustomerService customerService,
                                   ICaseSolutionService caseSolutionService,
                                   ISSOService ssoService
                                  ):base(masterDataService,ssoService)
        {
            this._customerService = customerService;
            this._caseSolutionService = caseSolutionService;
        }

        //
        // GET: /CoWorkers/

        public ActionResult Index(int customerId)
        {
            if (!CheckAndUpdateGlobalValues(customerId))
                return RedirectToAction("Index", "Error", new { message = "Customer is not specified!", errorCode = 202 });
            
            var curIdentity = SessionFacade.CurrentUserIdentity;
            if (curIdentity != null && curIdentity.EmployeeNumber != "")
            {
                var _amAPIService = new AMAPIService();
                var employeeList = AsyncHelpers.RunSync<string>(() => _amAPIService.GetEmployeeFor(curIdentity.EmployeeNumber));
                
                TempData["EmployeeList"] = employeeList.Split(',').ToList();
            }

            return View();
        }

        private bool CheckAndUpdateGlobalValues(int customerId)
        {
            if ((SessionFacade.CurrentCustomer != null && SessionFacade.CurrentCustomer.Id != customerId) ||
                (SessionFacade.CurrentCustomer == null))
            {
                var newCustomer = _customerService.GetCustomer(customerId);
                if (newCustomer == null)
                    return false;

                SessionFacade.CurrentCustomer = newCustomer;
            }

            if (SessionFacade.CurrentLanguageId == null)
              SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            ViewBag.PublicCustomerId = customerId;            
            ViewBag.PublicCaseTemplate = _caseSolutionService.GetCaseSolutions(customerId).ToList();
            return true;
        }
    }
}
