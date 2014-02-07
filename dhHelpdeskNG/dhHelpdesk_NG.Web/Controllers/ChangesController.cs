namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Models;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public class ChangesController : BaseController
    {
        private readonly IChangeService changeService;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly IUpdatedFieldSettingsFactory updatedFieldSettingsFactory;

        private readonly IChangesGridModelFactory changesGridModelFactory;

        private readonly ISearchModelFactory searchModelFactory;

        private readonly IChangeModelFactory changeModelFactory;

        private readonly IUpdatedChangeAggregateFactory updatedChangeAggregateFactory;

        private readonly INewChangeModelFactory newChangeModelFactory;

        private readonly INewChangeAggregateFactory newChangeAggregateFactory;

        private readonly IUserTemporaryFilesStorage userTemporaryFilesStorage;

        private readonly IUserEditorValuesStorage userEditorValuesStorage;

        private readonly ILogsModelFactory logsModelFactory;

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeService changeService,
            ISettingsModelFactory settingsModelFactory,
            IUpdatedFieldSettingsFactory updatedFieldSettingsFactory,
            IChangesGridModelFactory changesGridModelFactory,
            ISearchModelFactory searchModelFactory,
            IChangeModelFactory changeModelFactory,
            IUpdatedChangeAggregateFactory updatedChangeAggregateFactory,
            INewChangeModelFactory newChangeModelFactory,
            INewChangeAggregateFactory newChangeAggregateFactory,
            IUserEditorValuesStorageFactory userEditorValuesStorageFactory,
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
            ILogsModelFactory logsModelFactory)
            : base(masterDataService)
        {
            this.changeService = changeService;
            this.settingsModelFactory = settingsModelFactory;
            this.updatedFieldSettingsFactory = updatedFieldSettingsFactory;
            this.changesGridModelFactory = changesGridModelFactory;
            this.searchModelFactory = searchModelFactory;
            this.changeModelFactory = changeModelFactory;
            this.updatedChangeAggregateFactory = updatedChangeAggregateFactory;
            this.newChangeModelFactory = newChangeModelFactory;
            this.newChangeAggregateFactory = newChangeAggregateFactory;
            this.logsModelFactory = logsModelFactory;

            this.userEditorValuesStorage = userEditorValuesStorageFactory.Create(TopicName.Changes);
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create(TopicName.Changes);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int changeId, Subtopic subtopic, int logId)
        {
            this.userEditorValuesStorage.AddDeletedItemId(Enums.DeletedItemKey.DeletedLogs, logId);
            return this.RedirectToAction("Logs", new { changeId, subtopic });
        }

        [HttpGet]
        public PartialViewResult Logs(string changeId, Subtopic subtopic)
        {
            LogsModel model;

            if (GuidHelper.IsGuid(changeId))
            {
                model = new LogsModel(changeId, subtopic);
            }
            else
            {
                var deletedLogIds = this.userEditorValuesStorage.GetDeletedItemIds(Enums.DeletedItemKey.DeletedLogs);
                var id = int.Parse(changeId);
                var logs = this.changeService.FindLogs(id, subtopic, deletedLogIds);
                model = this.logsModelFactory.Create(id, subtopic, logs);
            }

            this.ViewData.TemplateInfo.HtmlFieldPrefix = "Input.Analyze";
            return this.PartialView(model);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public void Delete(int id)
        {
            this.changeService.DeleteChange(id);
            this.userTemporaryFilesStorage.DeleteFiles(id);

            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Registration.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Analyze.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Implementation.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Evaluation.ToString());

            this.userEditorValuesStorage.ClearDeletedItemIds(Enums.DeletedItemKey.DeletedLogs);
        }

        [HttpGet]
        public PartialViewResult Settings()
        {
            var fieldSettings = this.changeService.FindSettings(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var model = this.settingsModelFactory.Create(fieldSettings);
            return this.PartialView(model);
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Search()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var searchFieldSettings = this.changeService.FindSearchFieldSettings(currentCustomerId);

            List<ItemOverview> statuses = null;
            List<ItemOverview> objects = null;
            List<ItemOverview> workingGroups = null;
            List<ItemOverview> administrators = null;

            if (searchFieldSettings.Status.Show)
            {
                statuses = this.changeService.FindStatusOverviews(currentCustomerId);
            }

            if (searchFieldSettings.Object.Show)
            {
                objects = this.changeService.FindObjectOverviews(currentCustomerId);
            }

            if (searchFieldSettings.WorkingGroup.Show)
            {
                workingGroups = this.changeService.FindActiveWorkingGroupOverviews(currentCustomerId);
            }

            if (searchFieldSettings.Administrator.Show)
            {
                administrators = this.changeService.FindActiveAdministratorOverviews(currentCustomerId);
            }

            var model = this.searchModelFactory.Create(
                searchFieldSettings,
                statuses,
                new List<int>(),
                objects,
                new List<int>(),
                workingGroups,
                new List<int>(),
                administrators,
                new List<int>(),
                ChangeStatus.Active,
                string.Empty,
                100);

            return this.PartialView(model);
        }

        [HttpPost]
        public void Settings(SettingsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedFieldSettings = this.updatedFieldSettingsFactory.Create(
                model,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId,
                DateTime.Now);

            this.changeService.UpdateSettings(updatedFieldSettings);
        }

        [HttpGet]
        public ViewResult NewChange()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            var editSettings = this.changeService.FindChangeEditSettings(customerId, languageId);
            var optionalData = this.changeService.FindNewChangeOptionalData(customerId);
            var model = this.newChangeModelFactory.Create(Guid.NewGuid().ToString(), optionalData, editSettings);
            return this.View(model);
        }

        [HttpPost]
        public void NewChange(NewChangeModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var registrationFiles = this.userTemporaryFilesStorage.GetFiles(
                inputModel.Id,
                Subtopic.Registration.ToString());

            var analyzeFiles = this.userTemporaryFilesStorage.GetFiles(inputModel.Id, Subtopic.Analyze.ToString());

            var implementationFiles = this.userTemporaryFilesStorage.GetFiles(
                inputModel.Id,
                Subtopic.Implementation.ToString());

            var evaluationFiles = this.userTemporaryFilesStorage.GetFiles(inputModel.Id, Subtopic.Evaluation.ToString());

            var newChange = this.newChangeAggregateFactory.Create(
                inputModel,
                registrationFiles,
                analyzeFiles,
                implementationFiles,
                evaluationFiles,
                SessionFacade.CurrentCustomer.Id,
                DateTime.Now);

            this.changeService.AddChange(newChange);
            this.userTemporaryFilesStorage.DeleteFiles(inputModel.Id);
        }

        [HttpGet]
        public ViewResult Change(int id)
        {
            this.userTemporaryFilesStorage.DeleteFiles(id);
            
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Registration.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Analyze.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Implementation.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(id, Subtopic.Evaluation.ToString());

            this.userEditorValuesStorage.ClearDeletedItemIds(Enums.DeletedItemKey.DeletedLogs);

            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            var editSettings = this.changeService.FindChangeEditSettings(customerId, languageId);
            var change = this.changeService.FindChange(id);
            var optionalData = this.changeService.FindChangeOptionalData(customerId, id, editSettings);
            var model = this.changeModelFactory.Create(change, optionalData, editSettings);

            return this.View(model);
        }

        [HttpPost]
        public void Change(ChangeModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var registrationSubtopic = Subtopic.Registration.ToString();
            var analyzeSubtopic = Subtopic.Analyze.ToString();
            var implementationSubtopic = Subtopic.Implementation.ToString();
            var evaluationSubtopic = Subtopic.Evaluation.ToString();

            var newRegistrationFiles = this.userTemporaryFilesStorage.GetFiles(inputModel.Id, registrationSubtopic);
            var newAnalyzeFiles = this.userTemporaryFilesStorage.GetFiles(inputModel.Id, analyzeSubtopic);
            var newImplementationFiles = this.userTemporaryFilesStorage.GetFiles(inputModel.Id, implementationSubtopic);
            var newEvaluationFiles = this.userTemporaryFilesStorage.GetFiles(inputModel.Id, evaluationSubtopic);

            var deletedRegistrationFiles = this.userEditorValuesStorage.GetDeletedFileNames(
                inputModel.Id,
                registrationSubtopic);

            var deletedAnalyzeFiles = this.userEditorValuesStorage.GetDeletedFileNames(inputModel.Id, analyzeSubtopic);

            var deletedImplementationFiles = this.userEditorValuesStorage.GetDeletedFileNames(
                inputModel.Id,
                implementationSubtopic);

            var deletedEvaluationFiles = this.userEditorValuesStorage.GetDeletedFileNames(
                inputModel.Id,
                evaluationSubtopic);

            var deletedLogIds = this.userEditorValuesStorage.GetDeletedItemIds(Enums.DeletedItemKey.DeletedLogs);

            var newSubitems = new ChangeNewSubitems(
                newRegistrationFiles,
                newAnalyzeFiles,
                newImplementationFiles,
                newEvaluationFiles);

            var deletedSubitems = new ChangeDeletedSubitems(
                deletedRegistrationFiles,
                deletedAnalyzeFiles,
                deletedImplementationFiles,
                deletedEvaluationFiles,
                deletedLogIds);

            var updatedChange = this.updatedChangeAggregateFactory.Create(
                inputModel,
                newSubitems,
                deletedSubitems,
                DateTime.Now);

            this.changeService.UpdateChange(updatedChange);
            this.userTemporaryFilesStorage.DeleteFiles(inputModel.Id);

            this.userEditorValuesStorage.ClearDeletedFileNames(inputModel.Id, Subtopic.Registration.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(inputModel.Id, Subtopic.Analyze.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(inputModel.Id, Subtopic.Implementation.ToString());
            this.userEditorValuesStorage.ClearDeletedFileNames(inputModel.Id, Subtopic.Evaluation.ToString());

            this.userEditorValuesStorage.ClearDeletedItemIds(Enums.DeletedItemKey.DeletedLogs);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ChangesGrid(SearchModel searchModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var searchParameters = new SearchParameters(
                currentCustomerId,
                searchModel.StatusIds,
                searchModel.ObjectIds,
                searchModel.OwnerIds,
                searchModel.WorkingGroupIds,
                searchModel.AdministratorIds,
                searchModel.Pharse,
                searchModel.ShowValue,
                searchModel.RecordsOnPage);

            var searchResult = this.changeService.SearchDetailedChangeOverviews(searchParameters);
            
            var displaySettings = this.changeService.FindFieldOverviewSettings(
                currentCustomerId,
                SessionFacade.CurrentLanguageId);

            var model = this.changesGridModelFactory.Create(searchResult, displaySettings);
            return this.PartialView(model);
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string changeId, Subtopic subtopic)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(changeId))
            {
                fileNames = this.userTemporaryFilesStorage.GetFileNames(changeId, subtopic.ToString());
            }
            else
            {
                var fileNamesFromTemporaryStorage = this.userTemporaryFilesStorage.GetFileNames(
                    changeId,
                    subtopic.ToString());

                var deletedFileNames = this.userEditorValuesStorage.GetDeletedFileNames(
                    int.Parse(changeId),
                    subtopic.ToString());

                var fileNamesFromService = this.changeService.FindFileNames(
                    int.Parse(changeId),
                    subtopic,
                    deletedFileNames);

                fileNames = new List<string>(fileNamesFromTemporaryStorage.Count + fileNamesFromService.Count);

                fileNames.AddRange(fileNamesFromTemporaryStorage);
                fileNames.AddRange(fileNamesFromService);
            }

            var model = new FilesModel(changeId, subtopic, fileNames);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string changeId, Subtopic subtopic, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            var subtopicName = subtopic.ToString();

            if (this.userTemporaryFilesStorage.FileExists(name, changeId, subtopicName))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (GuidHelper.IsGuid(changeId))
            {
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, changeId, subtopicName);
            }
            else
            {
                if (this.changeService.FileExists(int.Parse(changeId), subtopic, name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.userTemporaryFilesStorage.AddFile(uploadedData, name, changeId, subtopicName);
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string changeId, Subtopic subtopic, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(changeId))
            {
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, changeId, subtopic.ToString());
            }
            else
            {
                var fileInWebStorage = this.userTemporaryFilesStorage.FileExists(
                    fileName,
                    changeId,
                    subtopic.ToString());

                fileContent = fileInWebStorage
                    ? this.userTemporaryFilesStorage.GetFileContent(fileName, changeId, subtopic.ToString())
                    : this.changeService.GetFileContent(int.Parse(changeId), subtopic, fileName);
            }

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string changeId, Subtopic subtopic, string fileName)
        {
            var subtopicName = subtopic.ToString();

            if (GuidHelper.IsGuid(changeId))
            {
                this.userTemporaryFilesStorage.DeleteFile(fileName, changeId, subtopicName);
            }
            else
            {
                if (this.userTemporaryFilesStorage.FileExists(fileName, changeId, subtopicName))
                {
                    this.userTemporaryFilesStorage.DeleteFile(fileName, changeId, subtopicName);
                }
                else
                {
                    this.userEditorValuesStorage.AddDeletedFileName(fileName, int.Parse(changeId), subtopicName);
                }
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }
    }
}