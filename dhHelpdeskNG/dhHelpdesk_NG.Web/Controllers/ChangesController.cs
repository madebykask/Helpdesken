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
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public sealed class ChangesController : BaseController
    {
        #region Fields

        private readonly IChangeModelFactory changeModelFactory;

        private readonly IChangeService changeService;

        private readonly IChangesGridModelFactory changesGridModelFactory;

        private readonly IUserEditorValuesStorage editorValuesStorage;

        private readonly ILogsModelFactory logsModelFactory;

        private readonly INewChangeModelFactory newChangeModelFactory;

        private readonly INewChangeRequestFactory newChangeRequestFactory;

        private readonly ISearchModelFactory searchModelFactory;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly IUserTemporaryFilesStorage temporaryFilesStorage;

        private readonly IUpdateChangeRequestFactory updateChangeRequestFactory;

        private readonly IUpdatedSettingsFactory updatedSettingFactory;

        #endregion

        #region Constructors and Destructors

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeModelFactory changeModelFactory,
            IChangeService changeService,
            IChangesGridModelFactory changesGridModelFactory,
            IUserEditorValuesStorage editorValuesStorage,
            ILogsModelFactory logsModelFactory,
            INewChangeRequestFactory newChangeRequestFactory,
            INewChangeModelFactory newChangeModelFactory,
            ISearchModelFactory searchModelFactory,
            ISettingsModelFactory settingsModelFactory,
            IUserTemporaryFilesStorage temporaryFilesStorage,
            IUpdateChangeRequestFactory updateChangeRequestFactory,
            IUpdatedSettingsFactory updatedSettingFactory)
            : base(masterDataService)
        {
            this.changeModelFactory = changeModelFactory;
            this.changeService = changeService;
            this.changesGridModelFactory = changesGridModelFactory;
            this.editorValuesStorage = editorValuesStorage;
            this.logsModelFactory = logsModelFactory;
            this.newChangeRequestFactory = newChangeRequestFactory;
            this.newChangeModelFactory = newChangeModelFactory;
            this.searchModelFactory = searchModelFactory;
            this.settingsModelFactory = settingsModelFactory;
            this.temporaryFilesStorage = temporaryFilesStorage;
            this.updateChangeRequestFactory = updateChangeRequestFactory;
            this.updatedSettingFactory = updatedSettingFactory;
        }

        #endregion

        #region Public Methods and Operators

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
        public ViewResult Change(int id)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            this.temporaryFilesStorage.DeleteFiles(id);
            this.editorValuesStorage.ClearDeletedFiles(id);
            this.editorValuesStorage.ClearDeletedItemIds(id, DeletedItemKey.DeletedLogs);

            var editSettings = this.changeService.GetChangeEditSettings(customerId, languageId);
            var response = this.changeService.FindChange(id);
            var editData = this.changeService.GetChangeEditData(id, customerId, editSettings);
            var model = this.changeModelFactory.Create(response, editData, editSettings);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void Change(InputModel model)
        {
            var id = int.Parse(model.ChangeId);

            var newRegistrationFiles = this.temporaryFilesStorage.GetFiles(id, SubtopicName.Registration);
            var newAnalyzeFiles = this.temporaryFilesStorage.GetFiles(id, SubtopicName.Analyze);
            var newImplementationFiles = this.temporaryFilesStorage.GetFiles(id, SubtopicName.Implementation);
            var newEvaluationFiles = this.temporaryFilesStorage.GetFiles(id, SubtopicName.Evaluation);

            var deletedRegistrationFiles = this.editorValuesStorage.GetDeletedFileNames(id, SubtopicName.Registration);
            var deletedAnalyzeFiles = this.editorValuesStorage.GetDeletedFileNames(id, SubtopicName.Analyze);
            var deletedImplementationFiles = this.editorValuesStorage.GetDeletedFileNames(
                id,
                SubtopicName.Implementation);
            var deletedEvaluationFiles = this.editorValuesStorage.GetDeletedFileNames(id, SubtopicName.Evaluation);

            var deletedLogIds = this.editorValuesStorage.GetDeletedItemIds(id, DeletedItemKey.DeletedLogs);

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
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId,
                DateTime.Now);

            this.changeService.UpdateChange(request);
            this.temporaryFilesStorage.DeleteFiles(id);
            this.editorValuesStorage.ClearDeletedFiles(id);
            this.editorValuesStorage.ClearDeletedItemIds(id, DeletedItemKey.DeletedLogs);
        }

        [HttpGet]
        public PartialViewResult Changes()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var languageId = SessionFacade.CurrentLanguageId;

            var settings = this.changeService.GetSearchSettings(customerId, languageId);
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

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult ChangesGrid(SearchModel searchModel)
        {
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

        [HttpPost]
        public void Delete(int id)
        {
            this.changeService.DeleteChange(id);
            this.temporaryFilesStorage.DeleteFiles(id);
            this.editorValuesStorage.ClearDeletedFiles(id);
            this.editorValuesStorage.ClearDeletedItemIds(id, DeletedItemKey.DeletedLogs);
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
                    this.editorValuesStorage.AddDeletedFile(fileName, id, subtopicName);
                }
            }

            return this.RedirectToAction("AttachedFiles", new { changeId, subtopic });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int changeId, Subtopic subtopic, int logId)
        {
            this.editorValuesStorage.AddDeletedItem(logId, DeletedItemKey.DeletedLogs, changeId);
            return this.RedirectToAction("Logs", new { changeId, subtopic });
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

        [HttpGet]
        public FileContentResult ExportChangesGridToExcelFile()
        {
            var filters = SessionFacade.GetPageFilters<ChangesFilter>(Enums.PageName.Changes);

            var parameters = new SearchParameters(
                SessionFacade.CurrentCustomer.Id,
                filters.StatusIds,
                filters.ObjectIds,
                filters.OwnerIds,
                filters.AffectedProcessIds,
                filters.WorkingGroupIds,
                filters.AdministratorIds,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage);

            var excelFile = this.changeService.ExportChangesToExcelFile(parameters, SessionFacade.CurrentLanguageId);
            return this.File(excelFile.Content, MimeType.ExcelFile, excelFile.Name);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var activeTab = SessionFacade.GetActiveTab(TopicName.Changes) ?? TabName.CaseSummary;
            var model = new IndexModel(activeTab);
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult Logs(int changeId, Subtopic subtopic)
        {
            var deletedLogIds = this.editorValuesStorage.GetDeletedItemIds(changeId, DeletedItemKey.DeletedLogs);
            var logs = this.changeService.FindLogs(changeId, subtopic, deletedLogIds);
            var model = this.logsModelFactory.Create(changeId, subtopic, logs);

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
        [BadRequestOnNotValid]
        public JsonResult NewChange(InputModel model)
        {
            var id = model.ChangeId;
            var registrationFiles = this.temporaryFilesStorage.GetFiles(id, SubtopicName.Registration);

            var request = this.newChangeRequestFactory.Create(
                model,
                registrationFiles,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId,
                DateTime.Now);

            this.changeService.AddChange(request);
            this.temporaryFilesStorage.DeleteFiles(id);

            return this.Json(request.Change.Id);
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
        [BadRequestOnNotValid]
        public RedirectToRouteResult Settings(SettingsModel settingsModel)
        {
            var settings = this.updatedSettingFactory.Create(
                settingsModel,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId,
                DateTime.Now);

            this.changeService.UpdateSettings(settings);
            return this.RedirectToAction("Changes");
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

        #endregion
    }
}