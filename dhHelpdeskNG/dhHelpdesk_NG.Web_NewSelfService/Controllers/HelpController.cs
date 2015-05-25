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
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.NewSelfService.Models.Help;

    public class HelpController : BaseController
    {
        private readonly ICustomerService _customerService;
        
        private readonly IInfoService _infoService;

        public HelpController(IMasterDataService masterDataService,
                              ICustomerService customerService,
                              ICaseSolutionService caseSolutionService,
                              IInfoService infoService
                             ): base(masterDataService, caseSolutionService)
        {            
            this._customerService = customerService;
            this._infoService = infoService;
        }

        public ActionResult Index(int customerId)
        {
            var htmlData = _infoService.GetInfoText((int) InfoTextType.SelfServiceHelp, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            var model = new HelpModel(htmlData == null ? string.Empty : htmlData.Name);
            return View(model);
        }              

        public ActionResult About(int customerId)
        {
            var htmlData = _infoService.GetInfoText((int) InfoTextType.SelfServiceAbout, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            var model = new AboutModel(htmlData == null? string.Empty : htmlData.Name);
            return View(model);
        }        
    }
}
