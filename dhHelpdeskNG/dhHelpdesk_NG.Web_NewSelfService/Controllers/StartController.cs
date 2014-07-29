using DH.Helpdesk.Domain;
using DH.Helpdesk.NewSelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.SSO.Input;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.NewSelfService.Models;
    using DH.Helpdesk.NewSelfService.Models.Case;
    using System.Security.Claims;
    using System.Configuration;

    public class StartController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ISSOService _ssoService;

        public StartController(IMasterDataService masterDataService,
                               ICustomerService customerService,
                               ICaseSolutionService caseSolutionService,
                               ISSOService ssoService
                              ):base(masterDataService,ssoService)
        {
            this._caseSolutionService = caseSolutionService;            
            this._customerService = customerService;
            //this._ssoService = ssoService;
        }

        //
        // GET: /Start/

        public ActionResult Index(int customerId = -1)
        {
            
            if (customerId == -1)
            {
               return RedirectToAction("Index", "Error", new { message = "Customer is not specified!  CustomerId = ?" });
            }

            if (!CheckAndUpdateGlobalValues(customerId))
            {
               return RedirectToAction("Index", "Error", new { message = "custom Message" });                
            }
            return this.View("Index");            
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
