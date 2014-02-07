namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Models;

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
            this._documentService = documentService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = this.IndexInputViewModel();

            return this.View(model);
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new Document { Customer_Id = SessionFacade.CurrentCustomer.Id });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Document document, int[] UsSelected, int[] WGsSelected)
        {
            if (this.Request.Files.Count > 0 && this.Request.Files[0].HasFile())
            {
                var file = this.Request.Files[0];
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
            this._documentService.SaveDocument(document, UsSelected, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "document");

            var model = this.CreateInputViewModel(document);

            return this.View(model);
        }

        public ActionResult NewCategory()
        {
            return this.View(new DocumentCategory() { Customer_Id = SessionFacade.CurrentCustomer.Id });
        }

        [HttpPost]
        public ActionResult NewCategory(DocumentCategory documentCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._documentService.SaveDocumentCategory(documentCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "document");

            return this.View(documentCategory);
        }

        public ActionResult Edit(int id)
        {
            var document = this._documentService.GetDocument(id);

            if (document == null)
                return new HttpNotFoundResult("No document found...");

            var model = this.CreateInputViewModel(document);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, int[] UsSelected, int[] WGsSelected)
        {
            Document d = this._documentService.GetDocument(id);

            if (this.Request.Files.Count > 0 && this.Request.Files[0].HasFile())
            {
                var file = this.Request.Files[0];
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

            this.UpdateModel(d, "document");

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._documentService.SaveDocument(d, UsSelected, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "document");

            var model = this.CreateInputViewModel(d);

            return this.View(model);
        }

        public ActionResult EditCategory(int id)
        {
            var documentCategory = this._documentService.GetDocumentCategory(id);

            if (documentCategory == null)
                return new HttpNotFoundResult("No document category found...");

            return this.View(documentCategory);
        }

        [HttpPost]
        public ActionResult EditCategory(DocumentCategory documentCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._documentService.SaveDocumentCategory(documentCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "document");

            return this.View(documentCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._documentService.DeleteDocument(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "document");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "document", new { id = id });
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (this._documentService.DeleteDocumentCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "document");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("editcategory", "document", new { id = id });
            }
        }

        private DocumentInputViewModel IndexInputViewModel()
        {
            var model = new DocumentInputViewModel
            {
                Documents = this._documentService.GetDocuments(SessionFacade.CurrentCustomer.Id),
                DocumentCategories = this._documentService.GetDocumentCategories(SessionFacade.CurrentCustomer.Id)
            };

            return model;
        }

        private DocumentInputViewModel CreateInputViewModel(Document document)
        {
            var usSelected = document.Us ?? new List<User>();
            var usAvailable = new List<User>();

            foreach (var us in this._userService.GetUsers())
            {
                if (!usSelected.Contains(us))
                    usAvailable.Add(us);
            }

            var wgsSelected = document.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            foreach (var wg in this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
            {
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
            }

            var model = new DocumentInputViewModel
            {
                Document = document,

                DocumentCats = this._documentService.GetDocumentCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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
            var fileToDelete = this._documentService.GetDocument(id);

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

            this._documentService.UpdateSavedFile(fileToDelete);

            return string.Empty;
        }
    }
}
