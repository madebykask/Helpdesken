using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Infrastructure.Extensions;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;

        public DocumentController(
            IDocumentService documentService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _documentService = documentService;
            _userService = userService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = IndexInputViewModel();

            return View(model);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new Document { Customer_Id = SessionFacade.CurrentCustomer.Id });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Document document, int[] UsSelected, int[] WGsSelected)
        {
            if (Request.Files.Count > 0 && Request.Files[0].HasFile())
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string contentType = file.ContentType;
                int intDocLen = file.ContentLength;
                byte[] docBuffer = new byte[intDocLen];
                Stream objStream = file.InputStream;
                objStream.Read(docBuffer, 0, intDocLen);

                document.ContentType = contentType;
                document.File = docBuffer;
                document.FileName = fileName;
                document.Size = intDocLen;
            }

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _documentService.SaveDocument(document, UsSelected, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "document");

            var model = CreateInputViewModel(document);

            return View(model);
        }

        public ActionResult NewCategory()
        {
            return View(new DocumentCategory() { Customer_Id = SessionFacade.CurrentCustomer.Id });
        }

        [HttpPost]
        public ActionResult NewCategory(DocumentCategory documentCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _documentService.SaveDocumentCategory(documentCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "document");

            return View(documentCategory);
        }

        public ActionResult Edit(int id)
        {
            var document = _documentService.GetDocument(id);

            if (document == null)
                return new HttpNotFoundResult("No document found...");

            var model = CreateInputViewModel(document);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, int[] UsSelected, int[] WGsSelected)
        {
            Document d = _documentService.GetDocument(id);

            if (Request.Files.Count > 0 && Request.Files[0].HasFile())
            {
                var file = Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string contentType = file.ContentType;
                int intDocLen = file.ContentLength;
                byte[] docBuffer = new byte[intDocLen];
                Stream objStream = file.InputStream;
                objStream.Read(docBuffer, 0, intDocLen);

                d.ContentType = contentType;
                d.File = docBuffer;
                d.FileName = fileName;
                d.Size = intDocLen;
            }

            UpdateModel(d, "document");

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _documentService.SaveDocument(d, UsSelected, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "document");

            var model = CreateInputViewModel(d);

            return View(model);
        }

        public ActionResult EditCategory(int id)
        {
            var documentCategory = _documentService.GetDocumentCategory(id);

            if (documentCategory == null)
                return new HttpNotFoundResult("No document category found...");

            return View(documentCategory);
        }

        [HttpPost]
        public ActionResult EditCategory(DocumentCategory documentCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _documentService.SaveDocumentCategory(documentCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "document");

            return View(documentCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_documentService.DeleteDocument(id) == DeleteMessage.Success)
                return RedirectToAction("index", "document");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "document", new { id = id });
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (_documentService.DeleteDocumentCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "document");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("editcategory", "document", new { id = id });
            }
        }

        private DocumentInputViewModel IndexInputViewModel()
        {
            var model = new DocumentInputViewModel
            {
                Documents = _documentService.GetDocuments(SessionFacade.CurrentCustomer.Id),
                DocumentCategories = _documentService.GetDocumentCategories(SessionFacade.CurrentCustomer.Id)
            };

            return model;
        }

        private DocumentInputViewModel CreateInputViewModel(Document document)
        {
            var usSelected = document.Us ?? new List<User>();
            var usAvailable = new List<User>();

            foreach (var us in _userService.GetUsers())
            {
                if (!usSelected.Contains(us))
                    usAvailable.Add(us);
            }

            var wgsSelected = document.WGs ?? new List<WorkingGroup>();
            var wgsAvailable = new List<WorkingGroup>();

            foreach (var wg in _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
            {
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
            }

            var model = new DocumentInputViewModel
            {
                Document = document,

                DocumentCats = _documentService.GetDocumentCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                UsAvailable = usAvailable.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.SurName,
                    Value = x.Id.ToString()
                }).ToList(),
                UsSelected = usSelected.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.SurName,
                    Value = x.Id.ToString()
                }).ToList(),
                WGsAvailable = wgsAvailable.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                WGsSelected = wgsSelected.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }

        [HttpPost]
        public string DeleteUploadedFile(int id)
        {
            var fileToDelete = _documentService.GetDocument(id);

            if (fileToDelete != null)
            {
                try
                {
                    fileToDelete.ContentType = "";
                    fileToDelete.File = null;
                    fileToDelete.FileName = "";
                    fileToDelete.Size = 0;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            _documentService.UpdateSavedFile(fileToDelete);

            return string.Empty;
        }
    }
}
