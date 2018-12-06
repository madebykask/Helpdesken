using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.BusinessData.Models.Shared;  

    public sealed class LicensesController : BaseController
    {
        private readonly ILicensesService licensesService;

        private readonly IWorkContext workContext;

        private readonly ILicensesModelFactory licensesModelFactory;

        private readonly ITemporaryFilesCache filesStore;

        private readonly IEditorStateCache filesStateStore;

        public LicensesController(
                IMasterDataService masterDataService, 
                ILicensesService licensesService, 
                IWorkContext workContext, 
                ILicensesModelFactory licensesModelFactory, 
                IEditorStateCacheFactory editorStateCacheFactory,
                ITemporaryFilesCacheFactory temporaryFilesCacheFactory)
            : base(masterDataService)
        {
            this.licensesService = licensesService;
            this.workContext = workContext;
            this.licensesModelFactory = licensesModelFactory;            
            this.filesStateStore = editorStateCacheFactory.CreateForModule(ModuleName.Licenses);
            this.filesStore = temporaryFilesCacheFactory.CreateForModule(ModuleName.Licenses);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var filters = SessionFacade.FindPageFilters<LicensesFilterModel>(PageName.LicensesLicenses);
            if (filters == null)
            {
                filters = LicensesFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.LicensesLicenses, filters);
            }

            var model = this.licensesModelFactory.GetIndexModel(filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Licenses(LicensesIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<LicensesFilterModel>(PageName.LicensesLicenses);

            SessionFacade.SavePageFilters(PageName.LicensesLicenses, filters);

            var licenses = this.licensesService.GetLicenses(this.workContext.Customer.CustomerId);

            var contentModel = this.licensesModelFactory.GetContentModel(licenses);
            return this.PartialView(contentModel);
        }

        [HttpGet]
        public ViewResult License(int? licenseId)
        {
            var data = this.licensesService.GetLicenseData(
                                            this.workContext.Customer.CustomerId,
                                            licenseId);          

            if (licenseId.HasValue)
            {
                var filesInDb = this.licensesService.GetLicenseFileNames(licenseId.Value);
                var filesOnDisc = this.filesStore.FindFiles(licenseId.Value).Select(f => f.Name).ToArray();
                foreach (var fileOnDisc in filesOnDisc)
                {
                    if (!filesInDb.Contains(fileOnDisc))
                    {
                        this.filesStore.DeleteFile(fileOnDisc, licenseId.Value);
                    } 
                }

                this.filesStateStore.ClearObjectDeletedFiles(licenseId.Value);
            }            

            var model = this.licensesModelFactory.GetEditModel(data);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult License(LicenseEditModel model, string entityId)
        {
            if (GuidHelper.IsGuid(entityId))
            {
                model.NewFiles = this.filesStore.FindFiles(entityId);
            }
            else
            {
                var filesInDb = this.licensesService.GetLicenseFileNames(model.Id);
                model.NewFiles = this.filesStore.FindFiles(entityId).Where(f => !filesInDb.Contains(f.Name)).ToList();
            }
            
            model.DeletedFiles = this.filesStateStore.FindDeletedFileNames(model.Id);
            
            var license = this.licensesModelFactory.GetBusinessModel(model);
            var licenseId = this.licensesService.AddOrUpdate(license);

            if (GuidHelper.IsGuid(entityId))
            {
                foreach (var newFile in model.NewFiles)
                {
                    this.filesStore.AddFile(newFile.Content, newFile.Name, licenseId);
                }

                this.filesStore.ResetCacheForObject(entityId);
            }
            else
            {
                foreach (var deletedFile in model.DeletedFiles)
                {
                    this.filesStore.DeleteFile(deletedFile, entityId);
                }

                this.filesStateStore.ClearObjectDeletedFiles(model.Id);                
            }    
            
            return this.RedirectToAction("License", new { licenseId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.licensesService.Delete(id);
            this.filesStateStore.ClearObjectDeletedFiles(id);   
            this.filesStore.ResetCacheForObject(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string entityId)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(entityId))
            {
                fileNames = this.filesStore.FindFileNames(entityId);
            }
            else
            {
                var id = int.Parse(entityId);
                var savedFiles = this.filesStore.FindFileNames(id);
                var deletedFiles = this.filesStateStore.FindDeletedFileNames(id);
                
                fileNames = new List<string>();
                fileNames.AddRange(savedFiles.Where(f => !deletedFiles.Contains(f)));
            }

            var model = new Models.Common.AttachedFilesModel(entityId, fileNames);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string entityId, string name)
        {
            var uploadedFile = this.Request.Files[0];
            if (uploadedFile == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null); 
            }

            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);

            if (this.filesStore.FileExists(name, entityId))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            this.filesStore.AddFile(fileContent, name, entityId);

            return this.RedirectToAction("AttachedFiles", new { entityId });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string entityId, string fileName)
        {
            if (!this.filesStore.FileExists(fileName, entityId))
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var fileContent = this.filesStore.GetFileContent(fileName, entityId);

            return this.File(fileContent, MimeType.BinaryFile, fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string entityId, string fileName)
        {
            if (GuidHelper.IsGuid(entityId))
            {
                this.filesStore.DeleteFile(fileName, entityId);
            }
            else
            {
                var id = int.Parse(entityId);
                this.filesStateStore.AddDeletedFile(fileName, id);
            }

            return this.RedirectToAction("AttachedFiles", new { entityId });
        }

        [HttpPost]
        public JsonResult GetJsonDepartmentsFor(int? regionId)
        {
            if (regionId.HasValue && regionId.Value == 0)
                regionId = null;

            var deps = this.licensesService.GetDepartmentsFor(this.workContext.Customer.CustomerId, regionId).ToList();
            return Json(deps.Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString())).OrderBy(d=> d.Name).ToList());
        }

    }
}
