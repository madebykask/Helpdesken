namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public PartialViewResult AttachedFiles(string changeId, Subtopic subtopic)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(changeId))
            {
                fileNames = this.temporaryFilesCache.FindFileNames(changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var temporaryFiles = this.temporaryFilesCache.FindFileNames(id, subtopic.ToString());
                var deletedFiles = this.editorStateCache.FindDeletedFileNames(id, subtopic.ToString());
                var savedFiles = this.changeService.FindChangeFileNamesExcludeDeleted(id, subtopic, deletedFiles);

                fileNames = new List<string>(temporaryFiles.Count + savedFiles.Count);
                fileNames.AddRange(temporaryFiles);
                fileNames.AddRange(savedFiles);
            }

            var model = new AttachedFilesModel(changeId, subtopic, fileNames);
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

            var registrationSubtopic = Subtopic.Registration.ToString();
            var analyzeSubtopic = Subtopic.Analyze.ToString();
            var implementationSubtopic = Subtopic.Implementation.ToString();
            var evaluationSubtopic = Subtopic.Evaluation.ToString();

            var newRegistrationFiles = this.temporaryFilesCache.FindFiles(id, registrationSubtopic);
            var newAnalyzeFiles = this.temporaryFilesCache.FindFiles(id, analyzeSubtopic);
            var newImplementationFiles = this.temporaryFilesCache.FindFiles(id, implementationSubtopic);
            var newEvaluationFiles = this.temporaryFilesCache.FindFiles(id, evaluationSubtopic);

            var deletedRegistrationFiles = this.editorStateCache.FindDeletedFileNames(id, registrationSubtopic);
            var deletedAnalyzeFiles = this.editorStateCache.FindDeletedFileNames(id, analyzeSubtopic);
            var deletedImplementationFiles = this.editorStateCache.FindDeletedFileNames(id, implementationSubtopic);
            var deletedEvaluationFiles = this.editorStateCache.FindDeletedFileNames(id, evaluationSubtopic);

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
        public RedirectToRouteResult DeleteFile(string changeId, Subtopic subtopic, string fileName)
        {
            if (GuidHelper.IsGuid(changeId))
            {
                this.temporaryFilesCache.DeleteFile(fileName, changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);

                if (this.temporaryFilesCache.FileExists(fileName, id, subtopic.ToString()))
                {
                    this.temporaryFilesCache.DeleteFile(fileName, id, subtopic.ToString());
                }
                else
                {
                    this.editorStateCache.AddDeletedFile(fileName, id, subtopic.ToString());
                }
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int changeId, Subtopic subtopic, int logId)
        {
            this.editorStateCache.AddDeletedItem(logId, ChangeDeletedItem.Logs, changeId);
            return this.RedirectToAction("Logs", new { changeId, subtopic });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string changeId, Subtopic subtopic, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(changeId))
            {
                fileContent = this.temporaryFilesCache.GetFileContent(fileName, changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var temporaryFiles = this.temporaryFilesCache.FileExists(fileName, id, subtopic.ToString());

                fileContent = temporaryFiles
                    ? this.temporaryFilesCache.GetFileContent(fileName, id, subtopic.ToString())
                    : this.changeService.GetFileContent(id, subtopic, fileName);
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
        public PartialViewResult Logs(int changeId, Subtopic subtopic)
        {
            var deletedLogIds = this.editorStateCache.GetDeletedItemIds(changeId, ChangeDeletedItem.Logs);
            var logs = this.changeService.FindChangeLogsExcludeSpecified(changeId, subtopic, deletedLogIds);
            
            var options = this.changeService.GetChangeEditData(
                changeId,
                SessionFacade.CurrentCustomer.Id,
                this.changeService.GetChangeEditSettings(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId));

            var model = this.logsModelFactory.Create(changeId, subtopic, logs, options);

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
            var registrationFiles = this.temporaryFilesCache.FindFiles(id, Subtopic.Registration.ToString());

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
        public RedirectToRouteResult UploadFile(string changeId, Subtopic subtopic, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);

            if (this.temporaryFilesCache.FileExists(name, changeId, subtopic.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (GuidHelper.IsGuid(changeId))
            {
                this.temporaryFilesCache.AddFile(fileContent, name, changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);

                if (this.changeService.FileExists(id, subtopic, name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.temporaryFilesCache.AddFile(fileContent, name, id, subtopic.ToString());
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }

        #endregion
    }
}