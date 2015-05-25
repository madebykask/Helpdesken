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
                                   IDocumentService documentsService)
                : base(masterDataService, caseSolutionService)
        {
             this._customerService = customerService;
             this._documentsService = documentsService;
        }

        //
        // GET: /Document/

        public ActionResult Index()
        {
            var model = new DocumentsModel();
            var customerId = SessionFacade.CurrentCustomer.Id;

            var cats = _documentsService.GetDocumentCategories(customerId).Where(d => d.ShowOnExternalPage).Select(d=> d.Id).ToList();            
            var docs = _documentsService.GetDocuments(customerId).Where(d => d.DocumentCategory_Id.HasValue && cats.Contains(d.DocumentCategory_Id.Value))
                                                                 .ToList();

            model.Documents = docs;
            return View(model);
        }
        
    }
}
