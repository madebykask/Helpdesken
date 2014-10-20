namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.Enums;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class LicensesController : BaseController
    {
        private readonly ILicensesService licensesService;

        private readonly IWorkContext workContext;

        private readonly ILicensesModelFactory licensesModelFactory;

        private readonly ITemporaryFilesCache temporaryFilesCache;

        private readonly IEditorStateCache editorStateCache;

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

            this.editorStateCache = editorStateCacheFactory.CreateForModule(ModuleName.Licenses);
            this.temporaryFilesCache = temporaryFilesCacheFactory.CreateForModule(ModuleName.Licenses);
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
            var model = this.licensesModelFactory.GetEditModel(data);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult License(LicenseEditModel model)
        {
            model.NewFiles = this.temporaryFilesCache.FindFiles(model.Id, AttachedFileType.License.ToString());
            model.DeletedFiles = this.editorStateCache.FindDeletedFileNames(model.Id, AttachedFileType.License.ToString());
            
            var license = this.licensesModelFactory.GetBusinessModel(model);
            var licenseId = this.licensesService.AddOrUpdate(license);
            this.temporaryFilesCache.ResetCacheForObject(model.Id);

            return this.RedirectToAction("License", new { licenseId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.licensesService.Delete(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string entityId, AttachedFileType? type)
        {
            if (type == null)
            {
                type = AttachedFileType.License;
            }

            List<string> fileNames;

            if (GuidHelper.IsGuid(entityId))
            {
                fileNames = this.temporaryFilesCache.FindFileNames(entityId, type.ToString());
            }
            else
            {
                var id = int.Parse(entityId);
                var temporaryFiles = this.temporaryFilesCache.FindFileNames(id, type.ToString());
                var deletedFiles = this.editorStateCache.FindDeletedFileNames(id, type.ToString());
                var savedFiles = this.licensesService.FindFileNamesExcludeSpecified(id, deletedFiles);

                fileNames = new List<string>(temporaryFiles.Count + savedFiles.Count);
                fileNames.AddRange(temporaryFiles);
                fileNames.AddRange(savedFiles);
            }

            var model = new Models.Common.AttachedFilesModel(entityId, type.Value, fileNames);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string entityId, AttachedFileType? type, string name)
        {
            if (type == null)
            {
                type = AttachedFileType.License;
            }

            var uploadedFile = this.Request.Files[0];
            if (uploadedFile == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null); 
            }

            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);

            if (this.temporaryFilesCache.FileExists(name, entityId, type.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (GuidHelper.IsGuid(entityId))
            {
                this.temporaryFilesCache.AddFile(fileContent, name, entityId, type.ToString());
            }
            else
            {
                var id = int.Parse(entityId);

                if (this.licensesService.FileExists(id, name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.temporaryFilesCache.AddFile(fileContent, name, id, type.ToString());
            }

            return this.RedirectToAction("AttachedFiles", new { entityId, type });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string entityId, AttachedFileType? type, string fileName)
        {
            if (type == null)
            {
                type = AttachedFileType.License;
            }

            byte[] fileContent;

            if (GuidHelper.IsGuid(entityId))
            {
                fileContent = this.temporaryFilesCache.GetFileContent(fileName, entityId, type.ToString());
            }
            else
            {
                var id = int.Parse(entityId);
                var temporaryFiles = this.temporaryFilesCache.FileExists(fileName, id, type.ToString());

                fileContent = temporaryFiles
                    ? this.temporaryFilesCache.GetFileContent(fileName, id, type.ToString())
                    : this.licensesService.GetFileContent(id, fileName);
            }

            return this.File(fileContent, MimeType.BinaryFile, fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string entityId, AttachedFileType? type, string fileName)
        {
            if (type == null)
            {
                type = AttachedFileType.License;
            }

            if (GuidHelper.IsGuid(entityId))
            {
                this.temporaryFilesCache.DeleteFile(fileName, entityId, type.ToString());
            }
            else
            {
                var id = int.Parse(entityId);

                if (this.temporaryFilesCache.FileExists(fileName, id, type.ToString()))
                {
                    this.temporaryFilesCache.DeleteFile(fileName, id, type.ToString());
                }
                else
                {
                    this.editorStateCache.AddDeletedFile(fileName, id, type.ToString());
                }
            }

            return this.RedirectToAction("AttachedFiles", new { entityId, type });
        }
    }
}
