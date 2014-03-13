using System.Globalization;
using DH.Helpdesk.BusinessData.Models.Document;
using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;    
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Models;

    public class ListType
    {
        public const int ltRoot = 0;

        public const int ltCategory = 1;

        public const int ltDocument = 2;
    }

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

        public ActionResult Index(int? listType, int Id = 0)
        {
            if (listType == null)
                listType = 0;

            var model = this.IndexInputViewModel(listType.Value, Id);
             
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

        [HttpGet]
        public JsonResult GetDocList(int docType, int Id)
        {

            var customerId = SessionFacade.CurrentCustomer.Id;

            var documents = new List<DocumentOverview>();

            switch (docType)
            {
                case ListType.ltRoot:
                    var categories = _documentService.GetDocumentCategories(customerId)
                                               .Select(c => new
                                               {
                                                   Id = c.Id,
                                                   DocName = c.Name,
                                                   Size = c.Documents.Count,
                                                   ChangeDate = c.ChangedDate,
                                                   UserName = " "
                                               }).OrderBy(c => c.DocName).ToList();

                    documents = categories.Select(c => new DocumentOverview(c.Id, c.DocName, c.Size, c.ChangeDate, c.UserName)).ToList();
                    break;

                case ListType.ltCategory:
                    var docs = _documentService.GetDocuments(customerId)
                                               .Where(c => c.DocumentCategory_Id == Id)
                                               .Select(c => new
                                               {
                                                   Id = c.Id,
                                                   DocName = c.Name,
                                                   Size = c.Size,
                                                   ChangeDate = c.ChangedDate,
                                                   UserName = (c.ChangedByUser == null) ? " " : c.ChangedByUser.FirstName + " " + c.ChangedByUser.SurName
                                               }).OrderBy(c => c.DocName).ToList();

                    documents = docs.Select(c => new DocumentOverview(c.Id, c.DocName, c.Size, c.ChangeDate, c.UserName)).ToList();
                    break;

            }

            //var faqOverviews = this.faqRepository.FindOverviewsByCategoryId(categoryId);

            //var faqModels =
            //    faqOverviews.Select(
            //        f => new FaqOverviewModel(f.Id, f.CreatedDate.ToString(CultureInfo.InvariantCulture), f.Text)).ToList();

            return this.Json(documents, JsonRequestBehavior.AllowGet);
        }


        private DocumentInputViewModel IndexInputViewModel(int docType, int Id)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var documents = new List<DocumentOverview>();

            switch (docType)
            {
                case ListType.ltRoot:
                    var categories = _documentService.GetDocumentCategories(customerId)
                                               .Select(c => new
                                               {
                                                   Id = c.Id,
                                                   DocName = c.Name,
                                                   Size = c.Documents.Count,
                                                   ChangeDate = c.ChangedDate,
                                                   UserName =  " "
                                               }).OrderBy(c => c.DocName).ToList();

                    documents = categories.Select(c => new DocumentOverview (c.Id, c.DocName, c.Size, c.ChangeDate, c.UserName)).ToList();                    
                    break;

                case ListType.ltCategory:
                    var docs = _documentService.GetDocuments(customerId)
                                               .Where(c=> c.DocumentCategory_Id == Id ) 
                                               .Select(c => new
                                               {
                                                   Id = c.Id,
                                                   DocName = c.Name,
                                                   Size = c.Size,
                                                   ChangeDate = c.ChangedDate,
                                                   UserName = (c.ChangedByUser == null)? " ": c.ChangedByUser.FirstName + " " + c.ChangedByUser.SurName
                                               }).OrderBy(c => c.DocName).ToList();

                    documents = docs.Select(c => new DocumentOverview(c.Id, c.DocName, c.Size, c.ChangeDate, c.UserName)).ToList();
                    break;

            }

            var docTree = _documentService.FindCategoriesWithSubcategoriesByCustomerId(customerId);            
            var categoryTreeItems = docTree.Select(this.CategoryToTreeItem).ToList();
            var categoriesTreeContent = new TreeContent(
                categoryTreeItems, "0");

            
            //var documents = 
            var model = new DocumentInputViewModel
            {
                CurrentDocType = docType,
                CurrentItemName = Id.ToString(),
                Documents = documents, //this._documentService.GetDocuments(SessionFacade.CurrentCustomer.Id),
                DocumentCategories = this._documentService.GetDocumentCategories(SessionFacade.CurrentCustomer.Id),
                DocumentTree = categoriesTreeContent
            };

            return model;
        }

        private TreeItem CategoryToTreeItem(CategoryWithSubCategory categoryWithSubcategories)
        {
            var item = new TreeItem(
                categoryWithSubcategories.Name, categoryWithSubcategories.Id.ToString(CultureInfo.InvariantCulture));

            if (categoryWithSubcategories.Subcategories.Any())
            {
                var subitems =
                    categoryWithSubcategories.Subcategories.Select(
                        this.CategoryToTreeItem).ToList();

                item.Children.AddRange(subitems);
            }

            return item;
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
