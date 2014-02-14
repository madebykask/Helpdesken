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
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public class ChangesController : BaseController
    {
        private readonly IChangeService changeService;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly IUpdatedFieldSettingsFactory updatedSettingFactory;

        private readonly IChangesGridModelFactory changesGridModelFactory;

        private readonly ISearchModelFactory searchModelFactory;

        private readonly IChangeModelFactory changeModelFactory;

        private readonly IUpdatedChangeAggregateFactory updateChangeRequestFactory;

        private readonly INewChangeModelFactory newChangeModelFactory;

        private readonly INewChangeRequestFactory newChangeRequestFactory;

        private readonly IUserTemporaryFilesStorage temporaryFilesStorage;

        private readonly IUserEditorValuesStorage editorValuesStorage;

        private readonly ILogsModelFactory logsModelFactory;

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeService changeService,
            ISettingsModelFactory settingsModelFactory,
            IUpdatedFieldSettingsFactory updatedSettingFactory,
            IChangesGridModelFactory changesGridModelFactory,
            ISearchModelFactory searchModelFactory,
            IChangeModelFactory changeModelFactory,
            IUpdatedChangeAggregateFactory updateChangeRequestFactory,
            INewChangeModelFactory newChangeModelFactory,
            INewChangeRequestFactory newChangeRequestFactory,
            IUserEditorValuesStorageFactory userEditorValuesStorageFactory,
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
            ILogsModelFactory logsModelFactory)
            : base(masterDataService)
        {
            this.changeService = changeService;
            this.settingsModelFactory = settingsModelFactory;
            this.updatedSettingFactory = updatedSettingFactory;
            this.changesGridModelFactory = changesGridModelFactory;
            this.searchModelFactory = searchModelFactory;
            this.changeModelFactory = changeModelFactory;
            this.updateChangeRequestFactory = updateChangeRequestFactory;
            this.newChangeModelFactory = newChangeModelFactory;
            this.newChangeRequestFactory = newChangeRequestFactory;
            this.logsModelFactory = logsModelFactory;

            this.editorValuesStorage = userEditorValuesStorageFactory.Create(TopicName.Changes);
            this.temporaryFilesStorage = userTemporaryFilesStorageFactory.Create(TopicName.Changes);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ChangesGrid(SearchModel searchModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            var filters = searchModel.ExtractFilters();
            SessionFacade.SavePageFilters(Enums.PageName.Changes, filters);

            var parameters = new SearchParameters(
                customerId,
                searchModel.StatusIds,
                searchModel.ObjectIds,
                searchModel.OwnerIds,
                searchModel.AffectedProcessIds,
                searchModel.WorkingGroupIds,
                searchModel.AdministratorIds,
                searchModel.Pharse,
                searchModel.StatusValue,
                searchModel.RecordsOnPage);

            var searchResult = this.changeService.Search(parameters);
            var overviewSettings = this.changeService.GetChangeOverviewSettings(customerId, languageId);
            var model = this.changesGridModelFactory.Create(searchResult, overviewSettings);
            
            return this.PartialView(model);
        }

        [HttpGet]
        public PartialViewResult Settings()
        {
            var settings = this.changeService.FindSettings(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var model = this.settingsModelFactory.Create(settings);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult Settings(SettingsModel settingsModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var settings = this.updatedSettingFactory.Create(
                settingsModel,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId,
                DateTime.Now);

            this.changeService.UpdateSettings(settings);
            return this.RedirectToAction("Changes");
        }

        [HttpGet]
        public PartialViewResult Changes()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var settings = this.changeService.GetSearchSettings(customerId);
            var data = this.changeService.GetSearchData(customerId);

            var filters = SessionFacade.GetPageFilters<ChangesFilter>(Enums.PageName.Changes);
            if (filters == null)
            {
                filters = ChangesFilter.CreateDefault();
                SessionFacade.SavePageFilters(Enums.PageName.Changes, filters);
            }

            var model = this.searchModelFactory.Create(filters, data, settings);
            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult NewChange()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            var editSettings = this.changeService.GetChangeEditSettings(customerId, languageId);
            var editData = this.changeService.GetNewChangeEditData(customerId, editSettings);
            var temporaryId = Guid.NewGuid().ToString();
            var model = this.newChangeModelFactory.Create(temporaryId, editData, editSettings);

            return this.View(model);
        }

        [HttpPost]
        public void NewChange(InputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var id = model.ChangeId;

            var registrationFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Registration);
            var analyzeFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Analyze);
            var implementationFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Implementation);
            var evaluationFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Evaluation);

            var request = this.newChangeRequestFactory.Create(
                model,
                registrationFiles,
                analyzeFiles,
                implementationFiles,
                evaluationFiles,
                SessionFacade.CurrentCustomer.Id,
                DateTime.Now);

            this.changeService.AddChange(request);
            this.temporaryFilesStorage.DeleteFiles(id);
        }

        [HttpPost]
        public void Delete(int id)
        {
            this.changeService.DeleteChange(id);
            this.temporaryFilesStorage.DeleteFiles(id);
            this.editorValuesStorage.ClearDeletedFileNames(id);
            this.editorValuesStorage.ClearDeletedItemIds(id, Enums.DeletedItemKey.DeletedLogs);
        }

        [HttpGet]
        public ViewResult Change(int id)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            this.temporaryFilesStorage.DeleteFiles(id);
            this.editorValuesStorage.ClearDeletedFileNames(id);
            this.editorValuesStorage.ClearDeletedItemIds(id, Enums.DeletedItemKey.DeletedLogs);

            var editSettings = this.changeService.GetChangeEditSettings(customerId, languageId);
            var response = this.changeService.FindChange(id);
            var editData = this.changeService.GetChangeEditData(id, customerId, editSettings);
            var model = this.changeModelFactory.Create(response, editData, editSettings);

            return this.View(model);
        }

        [HttpPost]
        public void Change(InputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var id = int.Parse(model.ChangeId);

            var newRegistrationFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Registration);
            var newAnalyzeFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Analyze);
            var newImplementationFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Implementation);
            var newEvaluationFiles = this.temporaryFilesStorage.GetFiles(id, Enums.SubtopicName.Evaluation);

            var deletedRegistrationFiles = this.editorValuesStorage.GetDeletedFileNames(id, Enums.SubtopicName.Registration);
            var deletedAnalyzeFiles = this.editorValuesStorage.GetDeletedFileNames(id, Enums.SubtopicName.Analyze);
            var deletedImplementationFiles = this.editorValuesStorage.GetDeletedFileNames(id, Enums.SubtopicName.Implementation);
            var deletedEvaluationFiles = this.editorValuesStorage.GetDeletedFileNames(id, Enums.SubtopicName.Evaluation);

            var deletedLogIds = this.editorValuesStorage.GetDeletedItemIds(id, Enums.DeletedItemKey.DeletedLogs);

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
                DateTime.Now);

            this.changeService.UpdateChange(request);
            this.temporaryFilesStorage.DeleteFiles(id);
            this.editorValuesStorage.ClearDeletedFileNames(id);
            this.editorValuesStorage.ClearDeletedItemIds(id, Enums.DeletedItemKey.DeletedLogs);
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string changeId, Subtopic subtopic)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(changeId))
            {
                fileNames = this.temporaryFilesStorage.GetFileNames(changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var subtopicName = subtopic.ToString();
                var temporaryFiles = this.temporaryFilesStorage.GetFileNames(id, subtopicName);
                var deletedFiles = this.editorValuesStorage.GetDeletedFileNames(id, subtopicName);
                var savedFiles = this.changeService.FindFileNames(id, subtopic, deletedFiles);

                fileNames = new List<string>(temporaryFiles.Count + savedFiles.Count);
                fileNames.AddRange(temporaryFiles);
                fileNames.AddRange(savedFiles);
            }

            var model = new AttachedFilesModel(changeId, subtopic, fileNames);
            return this.PartialView(model);
        }

        [HttpGet]
        public FileContentResult DownloadFile(string changeId, Subtopic subtopic, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(changeId))
            {
                fileContent = this.temporaryFilesStorage.GetFileContent(fileName, changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var subtopicName = subtopic.ToString();
                var temporaryFiles = this.temporaryFilesStorage.FileExists(fileName, id, subtopicName);

                fileContent = temporaryFiles
                    ? this.temporaryFilesStorage.GetFileContent(fileName, id, subtopicName)
                    : this.changeService.GetFileContent(id, subtopic, fileName);
            }

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string changeId, Subtopic subtopic, string fileName)
        {
            if (GuidHelper.IsGuid(changeId))
            {
                this.temporaryFilesStorage.DeleteFile(fileName, changeId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(changeId);
                var subtopicName = subtopic.ToString();

                if (this.temporaryFilesStorage.FileExists(fileName, id, subtopicName))
                {
                    this.temporaryFilesStorage.DeleteFile(fileName, id, subtopicName);
                }
                else
                {
                    this.editorValuesStorage.AddDeletedFileName(fileName, id, subtopicName);
                }
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string changeId, Subtopic subtopic, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            var subtopicName = subtopic.ToString();

            if (this.temporaryFilesStorage.FileExists(name, changeId, subtopicName))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (GuidHelper.IsGuid(changeId))
            {
                this.temporaryFilesStorage.AddFile(uploadedData, name, changeId, subtopicName);
            }
            else
            {
                var id = int.Parse(changeId);

                if (this.changeService.FileExists(id, subtopic, name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.temporaryFilesStorage.AddFile(uploadedData, name, id, subtopicName);
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int changeId, Subtopic subtopic, int logId)
        {
            this.editorValuesStorage.AddDeletedItemId(changeId, Enums.DeletedItemKey.DeletedLogs, logId);
            return this.RedirectToAction("Logs", new { changeId, subtopic });
        }

        [HttpGet]
        public PartialViewResult Logs(int changeId, Subtopic subtopic)
        {
            var deletedLogIds = this.editorValuesStorage.GetDeletedItemIds(changeId, Enums.DeletedItemKey.DeletedLogs);
            var logs = this.changeService.FindLogs(changeId, subtopic, deletedLogIds);
            var model = this.logsModelFactory.Create(changeId, subtopic, logs);

            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult Index()
        {
            return this.View();
        }
    }
}