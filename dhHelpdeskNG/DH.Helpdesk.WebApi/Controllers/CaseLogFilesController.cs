using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseLogFilesController : BaseApiController
    {
        private readonly ISettingsLogic _settingsLogic;
        private readonly ILogFileService _logFileService;
        private readonly ICaseFileService _caseFileService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
		private readonly IFeatureToggleService _featureToggleService;
		private readonly IFileViewLogService _fileViewLogService;

		public CaseLogFilesController(
            ILogFileService logFileService,
            ICaseFileService caseFileService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            ISettingsLogic settingsLogic, 
            ICaseFieldSettingService caseFieldSettingService,
			IFeatureToggleService featureToggleService,
			IFileViewLogService fileViewLogService)
        {
            _caseFileService = caseFileService;
            _logFileService = logFileService;
            _settingsLogic = settingsLogic;
            _caseFieldSettingService = caseFieldSettingService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
			_featureToggleService = featureToggleService;
			_fileViewLogService = fileViewLogService;

		}

        //ex: /api/Case/123/LogFile/1203?cid=1
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}/logfile/{fileId:int}")] 
        public Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int fileId, [FromUri]int cid, bool? inline = false)
        {
            byte[] content = null;
            var basePath = _settingsLogic.GetFilePath(cid);
            var fileInfo = _logFileService.GetFileDetails(fileId);
            var isCaseFile = fileInfo.IsCaseFile ?? false;

			var disableLogFileView = _featureToggleService.Get(Common.Constants.FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE);

			if (isCaseFile)
            {
				var model = _caseFileService.GetFileContentByIdAndFileName(caseId, basePath, fileInfo.FileName);

				if (!disableLogFileView.Active)
				{
                    var path = Path.GetDirectoryName(model.FilePath);
					_fileViewLogService.Log(caseId, UserId, fileInfo.FileName, path, FileViewLogFileSource.WebApi, FileViewLogOperation.View);
				}

				content = model.Content;
            }
            else
            {
                if (fileInfo.LogType == LogFileType.Internal)
                {
                    var setting = _caseFieldSettingService.GetCaseFieldSettingsByName(cid,
                            GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal.ToString().Replace("tblLog_", "tblLog."))
                        .FirstOrDefault(x => x.IsActive);
                    if (setting == null)
                        return Task.FromResult(Forbidden("Not allowed to view file."));
                } 
                var logFile = _logFileService.GetFileContentById(fileId, basePath, fileInfo.LogType);

				if (!disableLogFileView.Active)
                {
                    var path = Path.GetDirectoryName(logFile.Path);
					_fileViewLogService.Log(caseId, UserId, logFile.FileName, path, FileViewLogFileSource.WebApi, FileViewLogOperation.View);
				}

				content = logFile.Content;
            }

            if (content == null)
                SendResponse($"The case file '{fileInfo.FileName}' was not found", HttpStatusCode.NotFound);

            IHttpActionResult res = new FileResult(fileInfo.FileName, content, Request, inline ?? false);
            return Task.FromResult(res);
        }

        [HttpPost]
        [Route("{caseId}/logfile/")]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        public async Task<IHttpActionResult> UploadLogFile([FromUri]string caseId, [FromUri]int cid, [FromUri]LogFileType type)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var stream = filesReadToProvider.Contents.FirstOrDefault();
            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Unquote().Trim();

                //fix file name if exists
                var counter = 1;
                var newFileName = fileName;
                var moduleName = type == LogFileType.External ? ModuleName.Log : ModuleName.LogInternal;
                while(_userTemporaryFilesStorage.FileExists(newFileName, caseId, moduleName))
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(fileName)} ({counter++}){Path.GetExtension(fileName)}";
                }
                fileName = newFileName;

                _userTemporaryFilesStorage.AddFile(fileBytes, fileName, caseId, moduleName);
                return Ok(fileName);
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpDelete]
        [Route("{caseKey}/templogfile")]
        [SkipCustomerAuthorization] //skip check for new case
        public IHttpActionResult DeleteTempLogFile([FromUri]string caseKey, [FromUri]string fileName, [FromUri]LogFileType type)
        {
            var fileNameSafe = (fileName ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(fileNameSafe))
            {
                var moduleName = type == LogFileType.External ? ModuleName.Log : ModuleName.LogInternal;
                _userTemporaryFilesStorage.DeleteFile(fileNameSafe, caseKey, moduleName);
            }
            return Ok(true);
        }
    }
}
