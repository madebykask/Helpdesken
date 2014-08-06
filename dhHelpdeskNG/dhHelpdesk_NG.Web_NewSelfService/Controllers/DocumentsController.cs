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
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;

    public class DocumentsController : BaseController
    {
        private readonly ICustomerService _customerService;
    
  
        public DocumentsController(IMasterDataService masterDataService,
                                   ICustomerService customerService,
                                   ICaseSolutionService caseSolutionService,
                                   ISSOService ssoService)
                : base(masterDataService, ssoService, caseSolutionService)
        {
             this._customerService = customerService;
        }

        //
        // GET: /Document/

        public ActionResult Index(int customerId)
        {    
            return View();
        }
        
    }
}
