using DH.Helpdesk.Domain;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Models.Documents;

namespace DH.Helpdesk.SelfService.Controllers
{
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;    

    public class DocumentsController : BaseController
    {
        private readonly ICustomerService _customerService;

        private readonly IDocumentService _documentsService;        
    
        public DocumentsController(IMasterDataService masterDataService,
                                   ICustomerService customerService, 
                                   ISelfServiceConfigurationService configurationService,
                                   ICaseSolutionService caseSolutionService,
                                   IDocumentService documentsService)
                : base(configurationService, masterDataService, caseSolutionService)
        {         
            this._customerService = customerService;
            this._documentsService = documentsService;
        }

        //
        // GET: /Document/

        public ActionResult Index()
        {
            if (SessionFacade.CurrentCustomer == null)
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }

            var model = new DocumentsModel();
            var customerId = SessionFacade.CurrentCustomer.Id;            

            var cats = _documentsService.GetDocumentCategories(customerId).Where(d => d.ShowOnExternalPage).Select(d=> d.Id).ToList();            
            var docs = _documentsService.GetExternalPageDocuments(customerId)
                                        .Where(d => d.DocumentCategory_Id.HasValue && cats.Contains(d.DocumentCategory_Id.Value))
                                        .ToList();

            model.Documents = docs;
            return View(model);
        }

        [HttpGet]
        public ActionResult DocumentFile(int documentId)
        {
            var file = _documentsService.GetDocumentFile(documentId);
            if (file == null)
                return new HttpNotFoundResult();

            var contentType = file.ContentType;
            if (string.IsNullOrEmpty(contentType))
                contentType = "application/octet-stream";

            if (file.File == null || file.File.Length == 0)
                return new HttpNotFoundResult();

            return File(file.File, contentType, file.FileName);
        }
        
    }
}
