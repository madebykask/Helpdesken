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
        private readonly ISSOService _ssoService;

        public StartController(IMasterDataService masterDataService,
                               ICustomerService customerService,
                               ICaseSolutionService caseSolutionService,
                               ISSOService ssoService
                              ):base(masterDataService, ssoService, caseSolutionService)
        {
            
            this._customerService = customerService;            
        }

        //
        // GET: /Start/

        public  ActionResult Index(int customerId = -1)
        {
            return this.View("Index");            
        }
        
    }
}
