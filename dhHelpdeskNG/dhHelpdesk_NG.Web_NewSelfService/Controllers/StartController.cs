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
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.NewSelfService.Models;
    using DH.Helpdesk.NewSelfService.Models.Case;

    public class StartController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICaseSolutionService _caseSolutionService;

        public StartController(IMasterDataService masterDataService,
                               ICustomerService customerService,
                               ICaseSolutionService caseSolutionService
                              ):base(masterDataService)
        {
            this._caseSolutionService = caseSolutionService;            
            this._customerService = customerService;
        }

        //
        // GET: /Start/

        public ActionResult Index(int customerId)
        {
            if (!CheckAndUpdateGlobalValues(customerId))
                return null;        
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

            SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            ViewBag.PublicCustomerId = customerId;

            //var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var identity = User.Identity;
            if (identity != null)
            {
                SessionFacade.CurrentSystemUser = identity.Name.GetUserFromAdPath();
                ViewBag.PublicCaseTemplate = _caseSolutionService.GetCaseSolutions(customerId).ToList();
            }
            return true;
        }

    }
}
