using DH.Helpdesk.Domain;
using DH.Helpdesk.NewSelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.NewSelfService.Models.Documents;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;

    public class DocumentsController : BaseController
    {
        private readonly ICustomerService _customerService;

        private readonly IDocumentService _documentsService;
    
  
        public DocumentsController(IMasterDataService masterDataService,
                                   ICustomerService customerService,
                                   ICaseSolutionService caseSolutionService,
                                   IDocumentService documentsService,
                                   ISSOService ssoService)
                : base(masterDataService, ssoService, caseSolutionService)
        {
             this._customerService = customerService;
             this._documentsService = documentsService;
        }

        //
        // GET: /Document/

        public ActionResult Index()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var model = new DocumentsModel();
            model.Documents = _documentsService.GetDocuments(customerId).ToList();

            return View(model);
        }
        
    }
}
