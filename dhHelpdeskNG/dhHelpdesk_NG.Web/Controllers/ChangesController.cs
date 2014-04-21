namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Enums.Changes;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.CustomActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.SettingsEdit;

    public sealed class ChangesController : BaseController
    {
        #region Fields

        private readonly IChangeModelFactory changeModelFactory;

        private readonly IChangeService changeService;

        private readonly IChangesGridModelFactory changesGridModelFactory;

        private readonly IEditorStateCache editorStateCache;

        private readonly ILogsModelFactory logsModelFactory;

        private readonly INewChangeModelFactory newChangeModelFactory;

        private readonly INewChangeRequestFactory newChangeRequestFactory;

        private readonly ISearchModelFactory searchModelFactory;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly ITemporaryFilesCache temporaryFilesCache;

        private readonly IUpdateChangeRequestFactory updateChangeRequestFactory;

        private readonly IUpdatedSettingsFactory updatedSettingFactory;

        #endregion

        #region Constructors and Destructors

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeModelFactory changeModelFactory,
            IChangeService changeService,
            IChangesGridModelFactory changesGridModelFactory,
            IEditorStateCacheFactory editorStateCacheFactory,
            ILogsModelFactory logsModelFactory,
            INewChangeRequestFactory newChangeRequestFactory,
            INewChangeModelFactory newChangeModelFactory,
            ISearchModelFactory searchModelFactory,
            ISettingsModelFactory settingsModelFactory,
            ITemporaryFilesCacheFactory temporaryFilesCacheFactory,
            IUpdateChangeRequestFactory updateChangeRequestFactory,
            IUpdatedSettingsFactory updatedSettingFactory)
            : base(masterDataService)
        {
            this.changeModelFactory = changeModelFactory;
            this.changeService = changeService;
            this.changesGridModelFactory = changesGridModelFactory;
            this.logsModelFactory = logsModelFactory;
            this.newChangeRequestFactory = newChangeRequestFactory;
            this.newChangeModelFactory = newChangeModelFactory;
            this.searchModelFactory = searchModelFactory;
            this.settingsModelFactory = settingsModelFactory;
            this.updateChangeRequestFactory = updateChangeRequestFactory;
            this.updatedSettingFactory = updatedSettingFactory;

            this.editorStateCache = editorStateCacheFactory.CreateForModule(ModuleName.Changes);
            this.temporaryFilesCache = temporaryFilesCacheFactory.CreateForModule(ModuleName.Changes);
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        public PartialViewResult AttachedFiles(string changeId, ChangeArea area)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(changeId))
            {
                fileNames = this.temporaryFilesCache.FindFileNames(changeId, area.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var temporaryFiles = this.temporaryFilesCache.FindFileNames(id, area.ToString());
                var deletedFiles = this.editorStateCache.FindDeletedFileNames(id, area.ToString());
                var savedFiles = this.changeService.FindChangeFileNamesExcludeDeleted(id, area, deletedFiles);

                fileNames = new List<string>(temporaryFiles.Count + savedFiles.Count);
                fileNames.AddRange(temporaryFiles);
                fileNames.AddRange(savedFiles);
            }

            var model = new AttachedFilesModel(changeId, area, fileNames);
            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var operationContext = this.GetOperationContext();

            this.temporaryFilesCache.ResetCacheForObject(id);
            this.editorStateCache.ClearObjectDeletedItems(id, ChangeDeletedItem.Logs);

            var response = this.changeService.FindChange(id, operationContext);
            if (response == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }
            
            var model = this.changeModelFactory.Create(response);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void Edit(InputModel model)
        {
            var operationContext = this.GetOperationContext();

            var id = int.Parse(model.Id);

            var registrationArea = ChangeArea.Registration.ToString();
            var analyzeArea = ChangeArea.Analyze.ToString();
            var implementationArea = ChangeArea.Implementation.ToString();
            var evaluationArea = ChangeArea.Evaluation.ToString();

            var newRegistrationFiles = this.temporaryFilesCache.FindFiles(id, registrationArea);
            var newAnalyzeFiles = this.temporaryFilesCache.FindFiles(id, analyzeArea);
            var newImplementationFiles = this.temporaryFilesCache.FindFiles(id, implementationArea);
            var newEvaluationFiles = this.temporaryFilesCache.FindFiles(id, evaluationArea);

            var deletedRegistrationFiles = this.editorStateCache.FindDeletedFileNames(id, registrationArea);
            var deletedAnalyzeFiles = this.editorStateCache.FindDeletedFileNames(id, analyzeArea);
            var deletedImplementationFiles = this.editorStateCache.FindDeletedFileNames(id, implementationArea);
            var deletedEvaluationFiles = this.editorStateCache.FindDeletedFileNames(id, evaluationArea);

            var deletedLogIds = this.editorStateCache.GetDeletedItemIds(id, ChangeDeletedItem.Logs);

            var request = this.updateChangeRequestFactory.Create(
                model,
                deletedRegistrationFiles,
                deletedAnalyzeFiles,
                deletedImplementationFiles,
                deletedEvaluationFiles,
                deletedLogIds,
                newRegistrationFiles,
                newAnalyzeFiles,
                newImplementationFiles,
                newEvaluationFiles,
                operationContext);

            this.changeService.UpdateChange(request);
            this.temporaryFilesCache.ResetCacheForObject(id);
            this.editorStateCache.ClearObjectDeletedItems(id, ChangeDeletedItem.Logs);
        }

        [HttpGet]
        public PartialViewResult Changes()
        {
            return this.PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult ChangesGrid(SearchModel searchModel)
        {
            var operationContext = this.GetOperationContext();

            var filters = searchModel != null
                ? searchModel.ExtractFilters()
                : SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);

            SessionFacade.SavePageFilters(PageName.Changes, filters);

            var parameters = new SearchParameters(
                operationContext.CustomerId,
                filters.StatusIds,
                filters.ObjectIds,
                filters.OwnerIds,
                filters.AffectedProcessIds,
                filters.WorkingGroupIds,
                filters.AdministratorIds,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage);

            var response = this.changeService.Search(parameters, operationContext);
            var model = this.changesGridModelFactory.Create(response);

            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.changeService.DeleteChange(id);
            this.temporaryFilesCache.ResetCacheForObject(id);
            this.editorStateCache.ClearObjectDeletedItems(id, ChangeDeletedItem.Logs);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string changeId, ChangeArea area, string fileName)
        {
            if (GuidHelper.IsGuid(changeId))
            {
                this.temporaryFilesCache.DeleteFile(fileName, changeId, area.ToString());
            }
            else
            {
                var id = int.Parse(changeId);

                if (this.temporaryFilesCache.FileExists(fileName, id, area.ToString()))
                {
                    this.temporaryFilesCache.DeleteFile(fileName, id, area.ToString());
                }
                else
                {
                    this.editorStateCache.AddDeletedFile(fileName, id, area.ToString());
                }
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, area });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int changeId, ChangeArea area, int logId)
        {
            this.editorStateCache.AddDeletedItem(logId, ChangeDeletedItem.Logs, changeId);
            return this.RedirectToAction("Logs", new { changeId, area });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string changeId, ChangeArea area, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(changeId))
            {
                fileContent = this.temporaryFilesCache.GetFileContent(fileName, changeId, area.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var temporaryFiles = this.temporaryFilesCache.FileExists(fileName, id, area.ToString());

                fileContent = temporaryFiles
                    ? this.temporaryFilesCache.GetFileContent(fileName, id, area.ToString())
                    : this.changeService.GetFileContent(id, area, fileName);
            }

            return this.File(fileContent, MimeType.AnyFile, fileName);
        }

        [HttpGet]
        public FileContentResult ExportChangesGridToExcelFile()
        {
            var operationContext = this.GetOperationContext();
            var filters = SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);

            var parameters = new SearchParameters(
                operationContext.CustomerId,
                filters.StatusIds,
                filters.ObjectIds,
                filters.OwnerIds,
                filters.AffectedProcessIds,
                filters.WorkingGroupIds,
                filters.AdministratorIds,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage);

            var excelFile = this.changeService.ExportChangesToExcelFile(parameters, operationContext);
            return this.File(excelFile.Content, MimeType.ExcelFile, excelFile.Name);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var activeTab = SessionFacade.FindActiveTab(PageName.Changes) ?? TabName.CaseSummary;
            var model = new IndexModel(activeTab);
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult Logs(int changeId, ChangeArea area)
        {
            var deletedLogIds = this.editorStateCache.GetDeletedItemIds(changeId, ChangeDeletedItem.Logs);
            var logs = this.changeService.FindChangeLogsExcludeSpecified(changeId, area, deletedLogIds);
            
            var options = this.changeService.GetChangeEditData(
                changeId,
                SessionFacade.CurrentCustomer.Id,
                this.changeService.GetChangeEditSettings(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId));

            var model = this.logsModelFactory.Create(changeId, area, logs, options);

            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult New()
        {
            var operationContext = this.GetOperationContext();

            var temporaryId = Guid.NewGuid().ToString();
            var response = this.changeService.GetNewChangeEditData(operationContext);
            var model = this.newChangeModelFactory.Create(temporaryId, response);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public JsonResult New(InputModel model)
        {
            var operationContext = this.GetOperationContext();

            var id = model.Id;
            var registrationFiles = this.temporaryFilesCache.FindFiles(id, ChangeArea.Registration.ToString());

            var request = this.newChangeRequestFactory.Create(model, registrationFiles, operationContext);
            this.changeService.AddChange(request);
            this.temporaryFilesCache.ResetCacheForObject(id);

            return this.Json(request.Change.Id);
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Search()
        {
            var operationContext = this.GetOperationContext();

            var filters = SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);
            if (filters == null)
            {
                filters = ChangesFilter.CreateDefault();
                SessionFacade.SavePageFilters(PageName.Changes, filters);
            }

            var searchData = this.changeService.GetSearchData(operationContext);
            var model = this.searchModelFactory.Create(filters, searchData);
            return this.PartialView(model);
        }

        [HttpGet]
        public PartialViewResult Settings()
        {
            var operationContext = this.GetOperationContext();
            var settings = this.changeService.GetSettings(operationContext);
            var model = this.settingsModelFactory.Create(settings);

            return this.PartialView(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Settings(SettingsModel model)
        {
            var operationContext = this.GetOperationContext();
            var settings = this.updatedSettingFactory.Create(model, operationContext);
            this.changeService.UpdateSettings(settings);

            return this.RedirectToAction("Changes");
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string changeId, ChangeArea area, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);

            if (this.temporaryFilesCache.FileExists(name, changeId, area.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (GuidHelper.IsGuid(changeId))
            {
                this.temporaryFilesCache.AddFile(fileContent, name, changeId, area.ToString());
            }
            else
            {
                var id = int.Parse(changeId);

                if (this.changeService.FileExists(id, area, name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.temporaryFilesCache.AddFile(fileContent, name, id, area.ToString());
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, area });
        }

        #endregion
    }
}