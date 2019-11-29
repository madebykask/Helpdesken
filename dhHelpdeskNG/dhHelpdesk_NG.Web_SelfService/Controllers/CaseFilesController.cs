using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Extensions;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Extensions;
using DH.Helpdesk.SelfService.Infrastructure.Tools;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Controllers
{
    public class CaseFilesController: BaseController
    {
        private readonly ICaseService _caseService;
        private readonly ILogService _logService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IUserTemporaryFilesStorage _userTemporaryFilesStorage;
        private readonly ICaseFileService _caseFileService;
        private readonly ILogFileService _logFileService;
        private readonly IMasterDataService _masterDataService;
        private readonly IFilesStorage _filesStorage;
		private readonly IFileViewLogService _fileViewLogService;
		private readonly IFeatureToggleService _featureToggleService;

        public CaseFilesController(
            ICaseService caseService,
            ICaseFieldSettingService caseFieldSettingService,
            IMasterDataService masterDataService,
            ILogService logService,
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
            ICaseFileService caseFileService,
            ILogFileService logFileService,
            ICaseSolutionService caseSolutionService,
            ISelfServiceConfigurationService configurationService,
			IFileViewLogService fileViewLogService,
			IFeatureToggleService featureToggleService,
            IFilesStorage filesStorage)
            : base(configurationService, masterDataService, caseSolutionService)
        {
            _masterDataService = masterDataService;
            _caseService = caseService;
            _logService = logService;
            _caseFieldSettingService = caseFieldSettingService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create("Case");
            _caseFileService = caseFileService;
            _logFileService = logFileService;
			_fileViewLogService = fileViewLogService;
			_featureToggleService = featureToggleService;
            _filesStorage = filesStorage;
        }

        [HttpGet]
        public FileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
            {
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            }
            else
            {
                var caseId = int.Parse(id);
                var c = _caseService.GetCaseBasic(caseId);
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.CustomerId);

                var model = _caseFileService.GetFileContentByIdAndFileName(caseId, basePath, fileName);

                var disableLogFileView = _featureToggleService.Get(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE);
                if (!disableLogFileView.Active)
                {
                    var result = SessionFacade.CurrentUser != null
                        ? _fileViewLogService.Log(caseId, SessionFacade.CurrentUser.Id, fileName.Trim(), model.FilePath,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.View)
                        : _fileViewLogService.Log(caseId, GetUserName(), fileName.Trim(), model.FilePath,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.View);
                }

                fileContent = model.Content;
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public FileContentResult DownloadNewCaseFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var caseId = int.Parse(id);
                var c = _caseService.GetCaseBasic(caseId);
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.CustomerId);

                var model = _caseFileService.GetFileContentByIdAndFileName(caseId, basePath, fileName);

                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    var result = SessionFacade.CurrentUser != null
                        ? _fileViewLogService.Log(caseId, SessionFacade.CurrentUser.Id, fileName.Trim(), model.FilePath,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.View)
                        : _fileViewLogService.Log(caseId, GetUserName(), fileName.Trim(), model.FilePath,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.View);
                }

                fileContent = model.Content;
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public void UploadFile(string id, string name)
        {
            var uploadedData = Request.GetFileContent();
            if (uploadedData != null)
            {
                if (GuidHelper.IsGuid(id))
                {
                    if (_userTemporaryFilesStorage.FileExists(name, id))
                        throw new HttpException((int) HttpStatusCode.Conflict, null);

                    _userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                }
            }
        }

        [HttpPost]
        public void NewCaseUploadFile(string id, string name)
        {
            var uploadedData = Request.GetFileContent();
            if (uploadedData != null)
            {
                if (GuidHelper.IsGuid(id))
                {
                    if (_userTemporaryFilesStorage.FileExists(name, id))
                        throw new HttpException((int) HttpStatusCode.Conflict, null);

                    _userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                }
            }
        }

        [HttpPost]
        public void DeleteNewCaseFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var caseId = int.Parse(id);
                var c = _caseService.GetCaseBasic(caseId);
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.CustomerId);

                _caseFileService.DeleteByCaseIdAndFileName(caseId, basePath, fileName.Trim());
                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    var path = _filesStorage.ComposeFilePath(ModuleName.Cases, decimal.ToInt32(c.CaseNumber), basePath,
                        "");
                    var result = SessionFacade.CurrentUser != null
                        ? _fileViewLogService.Log(caseId, SessionFacade.CurrentUser.Id, fileName.Trim(), path,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.Delete)
                        : _fileViewLogService.Log(caseId, GetUserName(), fileName.Trim(), path,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.Delete);
                }
            }
        }

        [HttpGet]
        public JsonResult NewCaseFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                ? _userTemporaryFilesStorage.GetFileNames(id)
                : _caseFileService.FindFileNamesByCaseId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult DownloadLogFile(string id, string fileName, int? caseId)
        {
            var customer = caseId.HasValue && caseId > 0
                ? _caseService.GetCaseCustomer(caseId.Value)
                : SessionFacade.CurrentCustomer;
            var isTwoAttachmentsMode = IsTwoAttachmentsModeEnabled(customer.Id);
            var useInternalLogs = customer.UseInternalLogNoteOnExternalPage.ToBool();

            var logFileType = LogFileType.External;

            if (isTwoAttachmentsMode && useInternalLogs)
            {
                logFileType = LogFileType.Internal;
            }

            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
            {
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, logFileType.GetFolderPrefix());
            }
            else
            {
                var logId = int.Parse(id);

                var logFiles = _logFileService.GetLogFilesByNameAndId(fileName, logId);
                var logFile = logFiles
                    .OrderBy(o => o.LogType)
                    .FirstOrDefault(o => o.FileName == fileName);

                // Check that the found file also have set its logfiletype to Internal, if not use external for legacy support.
                if (logFile != null && isTwoAttachmentsMode && useInternalLogs &&
                    logFile.LogType == LogFileType.Internal)
                    logFileType = logFile.ParentLogType ?? logFile.LogType;
                else
                    logFileType = LogFileType.External;

                //existing file
                var basePath = _masterDataService.GetFilePath(customer.Id);
                var model = _logFileService.GetFileContentByIdAndFileName(logId, basePath, fileName, logFileType);

                fileContent = model.Content;

                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    var logCaseId = caseId ?? _logService.GetLogById(logId).CaseId;
                    var logSubFolder = logFileType.GetFolderPrefix();
                    var path = _filesStorage.ComposeFilePath(logSubFolder, logId, basePath, "");
                    var result = SessionFacade.CurrentUser != null
                        ? _fileViewLogService.Log(logCaseId, SessionFacade.CurrentUser.Id, fileName.Trim(), path,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.View)
                        : _fileViewLogService.Log(logCaseId, GetUserName(), fileName.Trim(), path,
                            FileViewLogFileSource.Selfservice, FileViewLogOperation.View);
                }
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public void UploadLogFile(string id, string name, int? caseId)
        {
            var customer = caseId.HasValue && caseId > 0
                ? _caseService.GetCaseCustomer(caseId.Value)
                : SessionFacade.CurrentCustomer;
            var isTwoAttachmentsMode = IsTwoAttachmentsModeEnabled(customer.Id);
            var isInternalLogUsed = customer.UseInternalLogNoteOnExternalPage.ToBool();
            var logSubFolder = isTwoAttachmentsMode && isInternalLogUsed ? ModuleName.LogInternal : ModuleName.Log;

            if (GuidHelper.IsGuid(id))
            {
                var fileContent = Request.GetFileContent();

                if (_userTemporaryFilesStorage.FileExists(name, id, logSubFolder))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }

                _userTemporaryFilesStorage.AddFile(fileContent, name, id, logSubFolder);
            }
        }

        [HttpGet]
        public JsonResult GetLogFiles(string id, int? caseId)
        {
            var customer = caseId.HasValue && caseId > 0
                ? _caseService.GetCaseCustomer(caseId.Value)
                : SessionFacade.CurrentCustomer;
            var isTwoAttachmentsMode = IsTwoAttachmentsModeEnabled(customer.Id);
            var isInternalLogUsed = customer.UseInternalLogNoteOnExternalPage.ToBool();

            //default log files values
            var logType = LogFileType.External;

            //load only internal log files if 2attachments and internalLog is used 
            if (isTwoAttachmentsMode && isInternalLogUsed)
                logType = LogFileType.Internal;

            var fileNames = GuidHelper.IsGuid(id)
                ? _userTemporaryFilesStorage.GetFileNames(id, logType.GetFolderPrefix())
                : _logFileService.FindFileNamesByLogId(int.Parse(id), logType);

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName, int? caseId)
        {
            if (GuidHelper.IsGuid(id))
            {
                var customer = caseId.HasValue && caseId > 0
                    ? _caseService.GetCaseCustomer(caseId.Value)
                    : SessionFacade.CurrentCustomer;
                var isTwoAttachmentsMode = IsTwoAttachmentsModeEnabled(customer.Id);
                var logSubFolder = isTwoAttachmentsMode && customer.UseInternalLogNoteOnExternalPage.ToBool()
                    ? ModuleName.LogInternal
                    : ModuleName.Log;

                // delete log file - check internal or external
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, logSubFolder);
            }
        }

        #region private 
        private string GetUserName()
        {
            return SessionFacade.CurrentUserIdentity?.UserId ??
                   SessionFacade.CurrentLocalUser?.UserId ?? "AnonymousUser";
        }

        private bool IsTwoAttachmentsModeEnabled(int customerId)
        {
            var fieldName = GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal.ToString().GetCaseFieldName();
            var isChecked = _caseFieldSettingService.GetCaseFieldSetting(customerId, fieldName)?.ShowExternal.ToBool() ?? false;
            return isChecked;
        }

        #endregion
    }
}