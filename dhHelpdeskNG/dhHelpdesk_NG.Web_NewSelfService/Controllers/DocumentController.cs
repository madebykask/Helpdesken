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
            if (!CheckAndUpdateGlobalValues(customerId))
                return null;

            return View();
        }

        private List<CaseSolution> GetCaseTemplates(int customerId, bool checkAuthentication = true)
        {
            var ret = new List<CaseSolution>();
            if (checkAuthentication)
            {
                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                //identity = null;
                if (identity == null)
                {
                    return ret;
                }
            }

            ret = _caseSolutionService.GetCaseSolutions(customerId).ToList();

            return ret;
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
            ViewBag.PublicCaseTemplate = GetCaseTemplates(customerId);

            return true;
        }        
    }
}
