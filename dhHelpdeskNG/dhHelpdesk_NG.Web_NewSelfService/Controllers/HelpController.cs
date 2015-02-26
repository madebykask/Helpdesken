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

    public class HelpController : BaseController
    {
        private readonly ICustomerService _customerService;
      
        public HelpController(IMasterDataService masterDataService,
                                     ICustomerService customerService,
                                     ICaseSolutionService caseSolutionService                                     
                                  )
            : base(masterDataService, caseSolutionService)
        {
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {      
            return View();
        }
       
        public ActionResult FindYourWay(int customerId)
        {         
            return View();
        }

        public ActionResult About(int customerId)
        {         
            return View();
        }        
    }
}
