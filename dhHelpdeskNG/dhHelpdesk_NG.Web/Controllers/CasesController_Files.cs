using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Logs;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Mvc;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.CaseLock;
using Microsoft.Ajax.Utilities;

namespace DH.Helpdesk.Web.Controllers
{
    public partial class CasesController
    {
        #region Public Methods

        [HttpGet]
        public ActionResult Files(string id, string savedFiles)
        {
            IList<CaseFileModel> caseFiles = new List<CaseFileModel>();
            var files = _userTemporaryFilesStorage.FindFileNamesAndDates(id, ModuleName.Cases);
            var tempCaseFiles = MakeCaseFileModel(files, savedFiles, true);

            if (!GuidHelper.IsGuid(id))
            {
                var canDelete = SessionFacade.CurrentUser.DeleteAttachedFilePermission.ToBool();
                caseFiles = _caseFileService.GetCaseFiles(int.Parse(id), canDelete);
            }

            if (tempCaseFiles != null && tempCaseFiles.Count > 0)
            {
                foreach (var tempCaseFile in tempCaseFiles)
                {
                    caseFiles.Add(tempCaseFile);
                }
            }

            var customerId = 0;
            if (!GuidHelper.IsGuid(id))
            {
                customerId = _caseService.GetCaseById(int.Parse(id)).Customer_Id;
            }

            var useVd = customerId != 0 &&
                        !string.IsNullOrEmpty(_masterDataService.GetVirtualDirectoryPath(customerId));

            var model = new CaseFilesModel(id, caseFiles.OrderBy(x => x.CreatedDate).ToArray(), savedFiles, useVd);
            return PartialView("_CaseFiles", model);
        }

        [HttpGet]
        //todo: check log files permission here
        public ActionResult LogFiles(string id, bool isInternalLog, int? caseId = null)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var currentUser = SessionFacade.CurrentUser;
            if (isInternalLog &&
                !CheckInternalLogFilesAccess(customerId, currentUser))
                return new HttpUnauthorizedResult("You are not authorized to access intenral log files");

            var subTopic = isInternalLog ? ModuleName.LogInternal : ModuleName.Log;
            var logType = isInternalLog ? LogFileType.Internal : LogFileType.External;
            
            BusinessData.Models.Case.Output.CaseOverview caseInfo = null;

            var existingFiles = new List<LogFileModel>();
            if (caseId.HasValue && caseId.Value > 0)
            {
                caseInfo = _caseService.GetCaseBasic(caseId.Value);
                var exFiles = _logFileService.GetExistingFileNamesByCaseId(caseId.Value, isInternalLog);
                existingFiles = exFiles.Select(f => new LogFileModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsExistCaseFile = f.IsExistCaseFile,
                    IsExistLogFile = f.IsExistLogFile,
                    ObjId = f.LogId ?? f.CaseId,
                    LogType = f.IsInternalLogNote ? LogFileType.Internal : LogFileType.External,
                    ParentLogType = f.LogType
                }).ToList();
            }

            if (GuidHelper.IsGuid(id))
            {
                var tempFiles = _userTemporaryFilesStorage.FindFileNames(id, subTopic);
                existingFiles.AddRange(tempFiles.Select(x => new LogFileModel
                {
                    Name = x,
                    LogType = logType,
                }));
            }
            else
            {
                var logFiles = _logFileService.GetLogFilesById(int.Parse(id), logType);
                existingFiles.AddRange(logFiles);
            }

            var virtualPathDir = _masterDataService.GetVirtualDirectoryPath(customerId);
            var model = new CaseLogFilesViewModel
            {
                CaseId = caseInfo?.Id ?? 0,
                FilesUrlBuilder = new CaseFilesUrlBuilder(virtualPathDir, RequestExtension.GetAbsoluteUrl()),
                LogId = id,
                IsExternal = logType == LogFileType.External,
                CaseNumber = Convert.ToInt32(caseInfo?.CaseNumber ?? 0.00M),
                Files = existingFiles,
                UseVirtualDirectory = !string.IsNullOrEmpty(virtualPathDir)
            };
            return PartialView("_CaseLogFiles", model);
        }

        [HttpPost]
        public void UploadCaseFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            //int caseId = 0;
            //if (GuidHelper.IsGuid(id))
            //{
            if (_userTemporaryFilesStorage.FileExists(name, id, ModuleName.Cases))
            {
                name = DateTime.Now.ToString() + '-' + name;
            }

            _userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Cases);
            //}
            //else if (Int32.TryParse(id, out caseId))
            //{
            //    if (_caseFileService.FileExists(int.Parse(id), name))
            //    {
            //        name = DateTime.Now.ToString() + '_' + name;
            //    }

            //    var customerId = _caseService.GetCaseCustomerId(caseId);
            //    var basePath = _masterDataService.GetFilePath(customerId);

            //    var caseFileDto = new CaseFileDto(
            //        uploadedData,
            //        basePath,
            //        name,
            //        DateTime.Now,
            //        int.Parse(id),
            //        _workContext.User.UserId);

            //    var path = "";
            //    _caseFileService.AddFile(caseFileDto, ref path);
            //    if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
            //    {
            //        var userId = SessionFacade.CurrentUser?.Id ?? 0;
            //        _fileViewLogService.Log(caseId, userId, caseFileDto.FileName, path, FileViewLogFileSource.Helpdesk,
            //            FileViewLogOperation.Add);
            //    }
            //}
        }

        [HttpPost]
        public void UploadLogFile(string id, string name, bool isInternalLog)
        {
            var uploadedFile = Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                var subTopic = isInternalLog ? ModuleName.LogInternal : ModuleName.Log;

                if (_userTemporaryFilesStorage.FileExists(name, id, subTopic))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }

                _userTemporaryFilesStorage.AddFile(uploadedData, name, id, subTopic);
            }
        }

        [HttpPost]
        public void DeleteCaseFile(string id, string fileName, bool isTemporary)
        {
            if (isTemporary)
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Cases);
            }
            else
            {
                var caseId = int.Parse(id);
                var c = _caseService.GetCaseById(caseId);
                var basePath = _masterDataService.GetFilePath(c.Customer_Id);

                _caseFileService.DeleteByCaseIdAndFileName(caseId, basePath, fileName.Trim());
                _invoiceArticleService.DeleteFileByCaseId(caseId, fileName.Trim());

                IDictionary<string, string> errors;
                var adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var extraField = new ExtraFieldCaseHistory {CaseFile = StringTags.Delete + fileName.Trim()};
                _caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5,
                    out errors, string.Empty, extraField);
                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    var userId = SessionFacade.CurrentUser?.Id ?? 0;
                    var path = _filesStorage.ComposeFilePath(ModuleName.Cases, decimal.ToInt32(c.CaseNumber), basePath, "");
                    _fileViewLogService.Log(caseId, userId, fileName.Trim(), path, FileViewLogFileSource.Helpdesk,
                        FileViewLogOperation.Delete);
                }
            }
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName, bool isExisting = false,
            LogFileType logType = LogFileType.External, int? fileId = null)
        {
            var logSubFolder = logType.GetFolderPrefix();

            if (fileId.HasValue)
            {
                if (fileId == 0 && !isExisting)
                {
                    if (GuidHelper.IsGuid(id))
                    {
                        _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, logSubFolder);
                    }
                }
                else if (isExisting)
                {
                    _logFileService.DeleteExistingById(fileId.Value);
                }
                else
                {
                    _logFileService.DeleteByFileIdAndFileName(fileId.Value, fileName.Trim());
                }
            }
            else
            {
                if (GuidHelper.IsGuid(id))
                {
                    _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, logSubFolder);
                }
                else
                {
                    var logId = int.Parse(id);
                    var log = _logService.GetLogById(logId);

                    if (log != null)
                    {
                        var case_ = _caseService.GetCaseById(log.CaseId);
                        var basePath = _masterDataService.GetFilePath(case_.Customer_Id);

                        _logFileService.DeleteByLogIdAndFileName(log.Id, basePath, fileName.Trim());

                        IDictionary<string, string> errors;
                        var adUser =
                            SessionFacade.CurrentUserIdentity
                                .UserId; //global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        if (case_ != null)
                        {
                            var extraField = new ExtraFieldCaseHistory {LogFile = StringTags.Delete + fileName.Trim()};
                            _caseService.SaveCaseHistory(case_, SessionFacade.CurrentUser.Id, adUser,
                                CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);
                        }
                        if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                        {
                            var userId = SessionFacade.CurrentUser?.Id ?? 0;
                            var path = _filesStorage.ComposeFilePath(logSubFolder, log.Id, basePath, "");
                            _fileViewLogService.Log(log.CaseId, userId, fileName.Trim(), path, FileViewLogFileSource.Helpdesk,
                                FileViewLogOperation.Delete);
                        }
                    }
                }
            }
        }

		[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
		public UnicodeFileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(id))
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases);
            else
            {
                var caseId = int.Parse(id);
                var c = _caseService.GetCaseBasic(caseId);
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.CustomerId);

                var model = _caseFileService.GetFileContentByIdAndFileName(caseId, basePath, fileName);
                fileContent = model.Content;

                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    var userId = SessionFacade.CurrentUser?.Id ?? 0;
                    _fileViewLogService.Log(caseId, userId, fileName, model.FilePath, FileViewLogFileSource.Helpdesk, FileViewLogOperation.View);

                }
            }

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)] // do not replace with [HttpGet, HttpHead]
        public ActionResult DownloadLogFile(string id, string fileName, LogFileType logType)
        {
            byte[] fileContent;

            if (logType == LogFileType.Internal &&
                !CheckInternalLogFilesAccess(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser))
                return new HttpUnauthorizedResult("You are not authorized to access intenral log files");

            if (GuidHelper.IsGuid(id))
            {
                var logFolder = logType.GetFolderPrefix();
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, logFolder);
            }
            else
            {
                var logId = int.Parse(id);
                var l = _logService.GetLogById(logId);
                var basePath = string.Empty;
                if (l != null)
                {
                    var c = _caseService.GetCaseBasic(l.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.CustomerId);
                }

                try
                {
                    var model = _logFileService.GetFileContentByIdAndFileName(logId, basePath, fileName,
                        logType);
                    fileContent = model.Content;
                    if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                    {
                        var userId = SessionFacade.CurrentUser?.Id ?? 0;
                        _fileViewLogService.Log(l.CaseId, userId, fileName, model.FilePath, FileViewLogFileSource.Helpdesk, FileViewLogOperation.View);
                    }
                }
                catch (FileNotFoundException e)
                {
                    return HttpNotFound("File not found");
                }
            }

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        private string GetCaseFileNames(int id)
        {
            var files = _caseFileService.FindFileNamesByCaseId(id);
            return string.Join("|", files);
        }

        [HttpPost]
        public ActionResult AttachExistingFile(List<CaseAttachedExFileModel> files, int caseId, bool isInternalLogNote)
        {
            if (files != null && files.Any())
            {
                var allFiles = files.Select(x => new LogExistingFileModel
                {
                    Name = x.FileName,
                    CaseId = x.CaseId,
                    IsExistLogFile = !x.IsCaseFile,
                    IsExistCaseFile = x.IsCaseFile,
                    LogId = x.LogId,
                    LogType = x.LogType,
                    IsInternalLogNote = isInternalLogNote
                });

                var success = _logFileService.SaveAttachedExistingLogFiles(allFiles, caseId, isInternalLogNote);
                return Json(new {success = true});
            }

            return Json(new {success = true});
            ;
        }

        [HttpGet]
        public ActionResult GetCaseFilesJS(int caseId)
        {
            var files = _caseFileService.FindFileNamesAndDatesByCaseId(caseId);
            var cfs = MakeCaseFileModel(files, string.Empty);
            var customerId = 0;
            customerId = _caseService.GetCaseById(caseId).Customer_Id;

            var useVD = customerId != 0 &&
                        !string.IsNullOrEmpty(_masterDataService.GetVirtualDirectoryPath(customerId));

            var model = new CaseFilesModel(caseId.ToString(), cfs.ToArray(), string.Empty, useVD);
            return PartialView("_CaseFiles", model);
        }

        [HttpGet]
        public ActionResult GetCaseExistingFilesToAttach(int caseId, bool isInternalLog)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var currentUser = SessionFacade.CurrentUser;
            if (isInternalLog &&
                !CheckInternalLogFilesAccess(customerId, currentUser))
                return new HttpUnauthorizedResult("You are not authorized to access internal log files");

            //todo: check log type access
            var canDelete = SessionFacade.CurrentUser.DeleteAttachedFilePermission.ToBool();
            var caseFiles = _caseFileService.GetCaseFiles(caseId, canDelete).OrderBy(x => x.CreatedDate);

            var attachedFiles = caseFiles.Select(f => new CaseAttachedExFileModel
            {
                Id = f.Id,
                FileName = f.FileName,
                IsCaseFile = true,
                CaseId = caseId,
                LogType = LogFileType.External // case files are always public
            }).ToList();

            //todo: filter files by log type 
            var exLogFiles = _logFileService.GetLogFilesByCaseId(caseId, isInternalLog).Select(f =>
                new CaseAttachedExFileModel
                {
                    Id = f.Id,
                    FileName = f.Name,
                    LogId = f.ObjId,
                    LogType = f.LogType
                }).ToList();

            attachedFiles.AddRange(exLogFiles);

            return PartialView("_CaseAttachExistFileList", attachedFiles);
        }

        [HttpGet]
        public ActionResult GetCaseInputModelForLog(int caseId)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            
            //todo: implement CreateCaseLogViewModel separate method to read only required data from db
            var model = GetCaseInputViewModel(userId, customerId, caseId, caseLockModel, caseFieldSettings);
            var caseLogViewModel = model.CreateCaseLogViewModel();
            return PartialView("_CaseLog", caseLogViewModel);
        }

        [HttpGet]
        public ActionResult GetCaseInputModelForHistory(int caseId)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            //todo: implement CreateHistoryViewModel separate method to read only required data from db
            var model = GetCaseInputViewModel(userId, customerId, caseId, caseLockModel, caseFieldSettings);
            var caseHistoryViewModel = model.CreateHistoryViewModel();
            return PartialView("_CaseHistory", caseHistoryViewModel);
        }

        private IList<CaseFileModel> MakeCaseFileModel(IList<CaseFileDate> files, string savedFiles, bool isTemporary = false)
        {
            var res = new List<CaseFileModel>();
            int i = 0;

            var savedFileList = string.IsNullOrEmpty(savedFiles) ? null : savedFiles.Split('|').ToList();

            foreach (var f in files)
            {
                i++;
                var canDelete = !(savedFileList != null && savedFileList.Contains(f.FileName));
                var cf = new CaseFileModel(i, i, f.FileName, f.FileDate, SessionFacade.CurrentUser.FirstName + " " + SessionFacade.CurrentUser.SurName, canDelete, isTemporary);
                res.Add(cf);
            }

            return res;
        }

        #endregion
    }
}