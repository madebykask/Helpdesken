using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web;
	using System.Web.Mvc;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Enums.Changes;
	using DH.Helpdesk.BusinessData.Models.Changes.Input;
	using DH.Helpdesk.Common.Tools;
	using DH.Helpdesk.Dal.Enums;
	using DH.Helpdesk.Dal.Infrastructure.Context;
	using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;
	using DH.Helpdesk.Services.Services;
	using DH.Helpdesk.Web.Enums;
	using DH.Helpdesk.Web.Enums.Changes;
	using DH.Helpdesk.Web.Infrastructure;
	using DH.Helpdesk.Web.Infrastructure.ActionFilters;
	using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
	using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
	using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
	using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange;
	using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange;
	using DH.Helpdesk.Web.Infrastructure.Tools;
	using DH.Helpdesk.Web.Models.Changes;
	using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
	using DH.Helpdesk.Web.Models.Changes.SettingsEdit;
	using DH.Helpdesk.Common.Enums;
	using System.IO;

	public sealed class ChangesController : UserInteractionController
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

        private readonly TemporaryIdProvider temporaryIdProvider;

        private readonly IEmailService emailService;

        private readonly IChangeStatusService changeStatusService;

        private readonly IWorkContext workContext;
		private readonly IGlobalSettingService _globalSettingService;

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
            IUpdatedSettingsFactory updatedSettingFactory,
            TemporaryIdProvider temporaryIdProvider, 
            IEmailService emailService, 
            IChangeStatusService changeStatusService, 
            IWorkContext workContext,
			IGlobalSettingService globalSettingService)
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
            this.temporaryIdProvider = temporaryIdProvider;
            this.emailService = emailService;
            this.changeStatusService = changeStatusService;
            this.workContext = workContext;
			_globalSettingService = globalSettingService;


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
        public PartialViewResult Changes(int? customerId, string filterType)
        {
            if (customerId.HasValue)
            {
                this.workContext.Customer.SetCustomer(customerId.Value);                
            }

            this.ViewData["CustomerId"] = customerId;

           

            var model = new SearchModel();
            if (filterType != null)
            {
                switch (filterType.ToLower())
                {
                    case FilterType.MyChanges:
                        var changeSettings = this.changeService.GetChangeEditSettings(SessionFacade.CurrentCustomer.Id, LanguageIds.Swedish);
                        if (changeSettings.Analyze != null && changeSettings.Analyze.Responsible.Show)
                            model.ResponsibleIds.Add(SessionFacade.CurrentUser.Id);
                        else
                            model.AdministratorIds.Add(SessionFacade.CurrentUser.Id);
                        break;

                    case FilterType.ActiveChanges:
                        model.StatusValue = ChangeStatus.Active;
                        break;

                    case FilterType.ClosedChanges:
                        model.StatusValue = ChangeStatus.Finished;
                        break;
                }
            }

            model.SortField = new Models.Shared.SortFieldModel();
            // TODO: Temporary RecordsOnPage set by hardcode 
            model.RecordsOnPage = 10;

            var filters = SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);
            if (filters == null)
                filters = ChangesFilter.CreateDefault();
            filters.Status = model.StatusValue;
            SessionFacade.SavePageFilters(PageName.Changes, filters);

            return this.PartialView(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult ChangesGrid(
                            SearchModel searchModel,
                            int? customerId)
        {

            var filters = searchModel != null
                ? searchModel.ExtractFilters()
                : SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);
            
            SessionFacade.SavePageFilters(PageName.Changes, filters);
            
            var parameters = new SearchParameters(
                customerId.HasValue ? customerId.Value : this.OperationContext.CustomerId,
                filters.StatusIds,
                filters.ObjectIds,
                filters.OwnerIds,
                filters.AffectedProcessIds,
                filters.WorkingGroupIds,
                filters.AdministratorIds,
                filters.ResponsibleIds,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage,
                filters.SortField);

            var response = this.changeService.Search(parameters, this.OperationContext);
            var model = this.changesGridModelFactory.Create(response, filters.SortField);

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

            return this.File(fileContent, MimeType.BinaryFile, fileName);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            this.temporaryFilesCache.ResetCacheForObject(id);
            this.editorStateCache.ClearObjectDeletedItems(id, ChangeDeletedItem.Logs);

            var response = this.changeService.FindChange(id, this.OperationContext);
            if (response == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var model = this.changeModelFactory.Create(response, this.OperationContext, _globalSettingService);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(InputModel model, string clickedButton)
        {
            var id = int.Parse(model.Id);

            var newRegistrationFiles = this.temporaryFilesCache.FindFiles(id, Subtopic.Registration.ToString());
            var newAnalyzeFiles = this.temporaryFilesCache.FindFiles(id, Subtopic.Analyze.ToString());
            var newImplementationFiles = this.temporaryFilesCache.FindFiles(id, Subtopic.Implementation.ToString());
            var newEvaluationFiles = this.temporaryFilesCache.FindFiles(id, Subtopic.Evaluation.ToString());

            var deletedRegistrationFiles = this.editorStateCache.FindDeletedFileNames(id, Subtopic.Registration.ToString());
            var deletedAnalyzeFiles = this.editorStateCache.FindDeletedFileNames(id, Subtopic.Analyze.ToString());
            var deletedImplementationFiles = this.editorStateCache.FindDeletedFileNames(id, Subtopic.Implementation.ToString());
            var deletedEvaluationFiles = this.editorStateCache.FindDeletedFileNames(id, Subtopic.Evaluation.ToString());

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
                this.OperationContext,
                this.emailService);

            this.changeService.UpdateChange(request);
            this.temporaryFilesCache.ResetCacheForObject(id);
            this.editorStateCache.ClearObjectDeletedItems(id, ChangeDeletedItem.Logs);

            if (clickedButton == ClickedButton.Save)
            {
                return this.RedirectToAction("Edit", new { id = id });
            }

            if (clickedButton == ClickedButton.SaveAndClose)
            {
                return this.RedirectToAction("Index");
            }

            throw new ArgumentOutOfRangeException("clickedButton");
        }

        [HttpGet]
        public FileContentResult ExportChangesGridToExcelFile()
        {
            var filters = SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);

            var parameters = new SearchParameters(
                this.OperationContext.CustomerId,
                filters.StatusIds,
                filters.ObjectIds,
                filters.OwnerIds,
                filters.AffectedProcessIds,
                filters.WorkingGroupIds,
                filters.AdministratorIds,
                filters.ResponsibleIds,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage,
                filters.SortField);

            var excelFile = this.changeService.ExportChangesToExcelFile(parameters, this.OperationContext);
            return this.File(excelFile.Content, MimeType.ExcelFile, excelFile.Name);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var defaultTab = TabName.Changes;
            var activeTab = SessionFacade.FindActiveTab(PageName.Changes) ?? defaultTab;
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
                this.changeService.GetChangeEditSettings(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId),
                this.OperationContext);

            var model = this.logsModelFactory.Create(changeId, subtopic, logs, options);

            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult New()
        {
            var temporaryId = this.temporaryIdProvider.ProvideTemporaryId();
            var response = this.changeService.GetNewChangeEditData(this.OperationContext);
            var statuses = this.changeStatusService.GetChangeStatuses(this.OperationContext.CustomerId);
            var model = this.newChangeModelFactory.Create(
                                            temporaryId, 
                                            response, 
                                            this.OperationContext,
                                            statuses);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(InputModel model, string clickedButton)
        {
            var registrationFiles = this.temporaryFilesCache.FindFiles(model.Id, Subtopic.Registration.ToString());
            var request = this.newChangeRequestFactory.Create(model, registrationFiles, this.OperationContext, this.emailService);
            this.changeService.AddChange(request);
            this.temporaryFilesCache.ResetCacheForObject(model.Id);

            if (clickedButton == ClickedButton.Save)
            {
                return this.RedirectToAction("Edit", new { id = request.Change.Id });
            }
            
            if (clickedButton == ClickedButton.SaveAndClose)
            {
                return this.RedirectToAction("Index");
            }
            
            throw new ArgumentOutOfRangeException("clickedButton");
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Search()
        {
            var filters = SessionFacade.FindPageFilters<ChangesFilter>(PageName.Changes);
            if (filters == null)
            {
                filters = ChangesFilter.CreateDefault();                
                SessionFacade.SavePageFilters(PageName.Changes, filters);
            }

            //if (status != null)
            //    filters.Status = status;
            var searchData = this.changeService.GetSearchData(this.OperationContext);
            var model = this.searchModelFactory.Create(filters, searchData);
            return this.PartialView(model);
        }

        [HttpGet]
        public PartialViewResult Settings(int? languageId)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            var response = this.changeService.GetSettings(languageId.Value, this.OperationContext);
            var model = this.settingsModelFactory.Create(response);
            
            return this.PartialView(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Settings(SettingsModel model)
        {
            var settings = this.updatedSettingFactory.Create(model, this.OperationContext);
            this.changeService.UpdateSettings(settings);
            
            return this.RedirectToAction("Changes");
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string changeId, Subtopic subtopic, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);


			var extension = Path.GetExtension(name);
			if (!_globalSettingService.IsExtensionInWhitelist(extension))
			{
				throw new ArgumentException($"File extension not valid: {name}");
			}

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