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

    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;    
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Documents;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public class TreeNodeType
    {
        public const int tnRoot = 0;
         
        public const int tnCategory = 1;        
    }   

    public class DocumentController : BaseController
    {        

        private readonly IDocumentService _documentService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ISettingService _settingService;
		private readonly IGlobalSettingService _globalSettingService;

		public DocumentController(
            IDocumentService documentService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IUserPermissionsChecker userPermissionsChecker,
            ISettingService settingService,
			IGlobalSettingService globalSettingService)
            : base(masterDataService)
        {
            this._documentService = documentService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            this._userPermissionsChecker = userPermissionsChecker;
            this._settingService = settingService;
			this._globalSettingService = globalSettingService;

		}

        [HttpPost]
        public void RememberTab(string topic, string tab)
        {
            SessionFacade.SaveActiveTab(topic, tab);
        }

        public ActionResult Index(int? listType, int Id = 0)
        {
            DocumentsFilterModel filters;
            if (listType == null)
            {
                filters = SessionFacade.FindPageFilters<DocumentsFilterModel>(PageName.DocumentsDocumentsList)
                          ?? DocumentsFilterModel.CreateDefault();
            }
            else
            {
                filters = new DocumentsFilterModel(listType.Value, Id);
            }

            SessionFacade.SavePageFilters(PageName.DocumentsDocumentsList, filters);

            var model = this.IndexInputViewModel(filters.DocumentType, filters.CategoryId);
             
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

				var extension = Path.GetExtension(fileName);
				if (!_globalSettingService.IsExtensionInWhitelist(extension))
				{
					throw new ArgumentException($"File extension not valid: {fileName}");
				}

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

            if (document.FileName == null)
            {
                document.FileName = "";
                document.ContentType = "";
            }

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
            var documents = GetDocumentOverview(docType, Id);

            TreeContent treeView = (TreeContent) HttpContext.Application["TreeView"];                     
            HttpContext.Application["TreeView"] = treeView;

            ViewBag.SelectedCategory = Id;
            ViewBag.SelectedListType = docType;

            var ds = SessionFacade.CurrentDocumentSearch;
            ds.Page = docType;
            ds.SearchDs = Id.ToString();            

            return this.Json(documents, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SortDocList(int docType, int Id, string sortedColumn)
        {            
            DocumentSearch ds = new DocumentSearch();
            if (SessionFacade.CurrentDocumentSearch == null)
            {
                ds.Ascending = true;
                ds.SortBy = "Name";                
            }
            else
            {
                ds = SessionFacade.CurrentDocumentSearch;
                if (ds.SortBy == sortedColumn)
                    ds.Ascending = !ds.Ascending;
                else
                    ds.SortBy = sortedColumn;
            }

            SessionFacade.CurrentDocumentSearch = ds;
            var documents = GetDocumentOverview(docType, Id);            
            TreeContent treeView = (TreeContent)HttpContext.Application["TreeView"];
            HttpContext.Application["TreeView"] = treeView;

            ViewBag.SelectedCategory = Id;
            ViewBag.SelectedListType = docType;
                       
            return this.Json(documents, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUploadedFile(int id)
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

            return this.RedirectToAction("edit", "document", new { id = id });
        }

        [HttpGet]
        public ActionResult DocumentFile(int document)
        {
            var file = _documentService.GetDocumentFile(document);
            if (file == null)
                return new HttpNotFoundResult();

            var contentType = file.ContentType;
            if (string.IsNullOrEmpty(contentType))
                contentType = "application/octet-stream";

            if (file.File == null || file.File.Length == 0)
                return new HttpNotFoundResult();

            return File(file.File, contentType, file.FileName);
        }

        private DocumentInputViewModel IndexInputViewModel(int docType, int Id)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var documents = GetDocumentOverview(docType, Id);
            var docTree = _documentService.FindCategoriesWithSubcategoriesByCustomerId(customerId);            
            var categoryTreeItems = docTree.Select(this.CategoryToTreeItem).ToList();
            var categoriesTreeContent = new TreeContent(categoryTreeItems, "0");

            var userHasDocumentAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.DocumentPermission);

            var activeTab = SessionFacade.FindActiveTab("Document");
            activeTab = (activeTab == null) ? "DocumentTab" : activeTab;

            var model = new DocumentInputViewModel(activeTab)
            {
                CurrentDocType = docType,
                CurrentItemName = Id.ToString(),
                Documents = documents,
                DocumentCategories = this._documentService.GetDocumentCategories(SessionFacade.CurrentCustomer.Id),
                DocumentTree = categoriesTreeContent,                
            };

            var ds = SessionFacade.CurrentDocumentSearch;
            HttpContext.Application["TreeView"] = categoriesTreeContent;
            ViewBag.SelectedCategory = Id;
            ViewBag.SelectedListType = docType;
            model.DocSearch = ds;

            model.UserHasDocumentAdminPermission = userHasDocumentAdminPermission;
			model.FileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();
            return model;
        }

        private List<DocumentOverview> GetDocumentOverview(int docType, int Id)
        {
            DocumentSearch ds = new DocumentSearch();
            if (SessionFacade.CurrentDocumentSearch == null)
            {
                ds.Ascending = true;
                ds.SortBy = "Name";
                SessionFacade.CurrentDocumentSearch = ds;
            }
            else
                ds = SessionFacade.CurrentDocumentSearch;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var documents = new List<DocumentOverview>();
            var cs = this._settingService.GetCustomerSetting(customerId);
            var isFirstName = (cs.IsUserFirstLastNameRepresentation == 1);

            if (SessionFacade.CurrentUser.UserGroupId > 2)
            {
                var docs = _documentService.GetDocumentsForAdministrators(customerId)
                                           .Select(c => new
                                           {
                                               Id = c.Id,
                                               DocName = c.Name,
                                               Size = c.Size,
                                               ChangeDate = c.ChangedDate,
                                               UserName = (c.ChangedByUser == null) ? " " :
                                                       (isFirstName ? string.Format("{0} {1}", c.ChangedByUser.FirstName, c.ChangedByUser.SurName) :
                                                                      string.Format("{0} {1}", c.ChangedByUser.SurName, c.ChangedByUser.FirstName)),

                                               CategoryId = c.DocumentCategory_Id
                                           })
                                           .ToList();

                if (docType == TreeNodeType.tnCategory)
                    docs = docs.Where(d => d.CategoryId == Id).ToList();

                switch (ds.SortBy)
                {
                    case "Size":
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.Size).ToList() : docs.OrderByDescending(d => d.Size).ToList();
                        break;

                    case "ChangedDate":
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.ChangeDate).ToList() : docs.OrderByDescending(d => d.ChangeDate).ToList();
                        break;

                    case "UserName":
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.UserName).ToList() : docs.OrderByDescending(d => d.UserName).ToList();
                        break;

                    default:
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.DocName).ToList() : docs.OrderByDescending(d => d.DocName).ToList();
                        break;
                }
                documents = docs.Select(c => new DocumentOverview(c.Id, c.DocName, c.Size.RoundQty(), c.ChangeDate, c.UserName)).ToList();
            }
            else
            {
                var docs = _documentService.GetDocuments(customerId)
                                           .Select(c => new
                                           {
                                               Id = c.Id,
                                               DocName = c.Name,
                                               Size = c.Size,
                                               ChangeDate = c.ChangedDate,
                                               UserName = (c.ChangedByUser == null) ? " " : c.ChangedByUser.SurName + " " + c.ChangedByUser.FirstName,
                                               CategoryId = c.DocumentCategory_Id
                                           })
                                           .ToList();

                if (docType == TreeNodeType.tnCategory)
                    docs = docs.Where(d => d.CategoryId == Id).ToList();

                switch (ds.SortBy)
                {
                    case "Size":
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.Size).ToList() : docs.OrderByDescending(d => d.Size).ToList();
                        break;

                    case "ChangedDate":
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.ChangeDate).ToList() : docs.OrderByDescending(d => d.ChangeDate).ToList();
                        break;

                    case "UserName":
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.UserName).ToList() : docs.OrderByDescending(d => d.UserName).ToList();
                        break;

                    default:
                        docs = (ds.Ascending) ? docs.OrderBy(d => d.DocName).ToList() : docs.OrderByDescending(d => d.DocName).ToList();
                        break;
                }
                documents = docs.Select(c => new DocumentOverview(c.Id, c.DocName, c.Size.RoundQty(), c.ChangeDate, c.UserName)).ToList();
            }
           

            //documents = docs.Select(c => new DocumentOverview(c.Id, c.DocName, c.Size.RoundQty(), c.ChangeDate, c.UserName)).ToList();
            return documents;
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
            var curCustomerId = SessionFacade.CurrentCustomer.Id;            
            var userHasDocumentAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.DocumentPermission);
            var customerSettings = this._settingService.GetCustomerSetting(curCustomerId);

            if (customerSettings.IsUserFirstLastNameRepresentation == 1)
            {
                foreach (var us in this._userService.GetUsers(curCustomerId).OrderBy(u => u.FirstName).ThenBy(u => u.SurName))                
                    if (!usSelected.Contains(us))
                        usAvailable.Add(us);

                usSelected = usSelected.OrderBy(us => us.FirstName).ThenBy(us => us.SurName).ToList();
            }
            else
            {
                foreach (var us in this._userService.GetUsers(curCustomerId).OrderBy(u => u.SurName).ThenBy(u => u.FirstName))               
                    if (!usSelected.Contains(us))
                        usAvailable.Add(us);

                usSelected = usSelected.OrderBy(us => us.SurName).ThenBy(us => us.FirstName).ToList();
            }
            
            var wgsSelected = document.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            foreach (var wg in this._workingGroupService.GetWorkingGroups(curCustomerId))
            {
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
            }

            var model = new DocumentInputViewModel
            {
                Document = document,

                DocumentCats = this._documentService.GetDocumentCategories(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                UsAvailable = usAvailable.Select(x => new SelectListItem
                {
                    Text = (customerSettings.IsUserFirstLastNameRepresentation == 1 ? 
                                string.Format("{0} {1}", x.FirstName, x.SurName) : 
                                string.Format("{0} {1}", x.SurName, x.FirstName)),
                    Value = x.Id.ToString()
                }).ToList(),

                UsSelected = usSelected.Select(x => new SelectListItem
                {
                    Text = (customerSettings.IsUserFirstLastNameRepresentation == 1 ?
                                string.Format("{0} {1}", x.FirstName, x.SurName) :
                                string.Format("{0} {1}", x.SurName, x.FirstName)),
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

            model.UserHasDocumentAdminPermission = userHasDocumentAdminPermission;
			model.FileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();
            //model.ShowOnStartPage = document.ShowOnStartPage;

            return model;
        }
        
    }
}
