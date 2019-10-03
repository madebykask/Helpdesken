using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.Common.Enums.FileViewLog;
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
    public class CaseFilesController : BaseApiController
    {
        private readonly ICaseFileService _caseFileService;
        private readonly ICaseService _caseService;
        private readonly ISettingsLogic _settingsLogic;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;
		private readonly IFileViewLogService _fileViewLogService;
		private readonly IFeatureToggleService _featureToggleService;

		#region ctor()

		public CaseFilesController(ICaseService caseService,
            ICaseFileService caseFileService,
            ISettingsLogic settingsLogic,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
			IFileViewLogService fileViewLogService,
			IFeatureToggleService featureToggleService)
        {
            _settingsLogic = settingsLogic;
            _caseService = caseService;
            _caseFileService = caseFileService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
			_fileViewLogService = fileViewLogService;
			_featureToggleService = featureToggleService;

		}

        #endregion
      
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}/file/{fileId:int}")] //ex: /api/Case/123/File/1203?cid=1
        public Task<IHttpActionResult> DownloadExistingFile([FromUri] int caseId, [FromUri] int fileId, [FromUri] int cid, bool? inline = false)
        {
            var fileContent = _caseFileService.GetCaseFile(cid, caseId, fileId, true); //TODO: async

            IHttpActionResult res = new FileResult(fileContent.FileName, fileContent.Content, Request, inline ?? false);
            return Task.FromResult(res);
        }

        [HttpGet]
        [Route("{caseKey:guid}/file")] 
        public Task<IHttpActionResult> DownloadTempFile([FromUri] Guid caseKey, [FromUri] string fileName, [FromUri] int cid, bool? inline = false)
        {
            var fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, caseKey.ToString(), "");
            IHttpActionResult res = new FileResult(fileName, fileContent, Request, inline ?? false);
            return Task.FromResult(res);
        }

        [HttpPost]
        [Route("{caseKey:guid}/file")] // remember to update WebApiCorsPolicyProvider if url is changed
        public async Task<IHttpActionResult> UploadNewCaseFile([FromUri] Guid caseKey)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var stream = filesReadToProvider.Contents.FirstOrDefault();
            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Unquote().Trim();

                var counter = 1;
                var newFileName = fileName;
                while (_userTemporaryFilesStorage.FileExists(fileName, caseKey.ToString()))
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(fileName)} ({counter++}){Path.GetExtension(fileName)}";
                }
                fileName = newFileName;

                _userTemporaryFilesStorage.AddFile(fileBytes, fileName, caseKey.ToString());
                return Ok(fileName);
            }

            return BadRequest("Failed to upload a file");
        }
        
        [HttpPost]
        [Route("{caseId:int}/file")] // remember to update WebApiCorsPolicyProvider if url is changed
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        public async Task<IHttpActionResult> UploadCaseFile([FromUri]int caseId, [FromUri]int cid)
        {
            var now = DateTime.Now;
            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            
            var customerId = _caseService.GetCaseCustomerId(caseId);
            var basePath = GetBasePath(customerId);

            var stream = filesReadToProvider.Contents.FirstOrDefault();

            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Unquote().Trim();

                //fix file name if exists
                var counter = 1;
                var newFileName = fileName;
                while (_caseFileService.FileExists(caseId, newFileName))
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(fileName)} ({counter++}){Path.GetExtension(fileName)}";
                }
                fileName = newFileName;

                var caseFileDto = new CaseFileDto(
                    fileBytes,
                    basePath,
                    fileName,
                    now,
                    caseId,
                    UserId);

				string path = "";
                var fileId = _caseFileService.AddFile(caseFileDto, ref path);

				var disableLogFileView = _featureToggleService.Get(Common.Constants.FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE);
				if (!disableLogFileView.Active)
				{
					_fileViewLogService.Log(caseId, UserId, caseFileDto.FileName, path, FileViewLogFileSource.WebApi, FileViewLogOperation.Add);
				}

				return Ok(new { id = fileId, name = fileName});
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpDelete]
        [Route("{caseKey:guid}/file")]
        public IHttpActionResult DeleteNewCaseFile([FromUri]Guid caseKey, [FromUri]string fileName)
        {
            //todo: make Async

            //todo: check if UriDecode is required for fileName
            var fileNameSafe = (fileName ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(fileNameSafe))
            {
                _userTemporaryFilesStorage.DeleteFile(fileNameSafe, caseKey.ToString());
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{caseId:int}/file/{fileId:int}")]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [CheckUserPermissions(UserPermission.DeleteAttachedFilePermission)]
        public async Task<IHttpActionResult> DeleteCaseFile(int caseId, int fileId)
        {
            var c = await _caseService.GetCaseByIdAsync(caseId);
            var customerId = c.Customer_Id;
            var basePath = GetBasePath(customerId);

            //todo: async
            var caseFileInfo = _caseFileService.GetCaseFile(caseId, fileId);
            _caseFileService.DeleteByCaseIdAndFileName(caseId, basePath, caseFileInfo.FileName);

            //todo: ?
            //_invoiceArticleService.DeleteFileByCaseId(int.Parse(id), fileName.Trim());

            IDictionary<string, string> errors;
            var adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            _caseService.SaveFileDeleteHistory(c, caseFileInfo.FileName, UserId, adUser, out errors);

            //todo: return  errors?
            return Ok();
        }
        
        [HttpDelete]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}/tempfiles")]
        public IHttpActionResult CleanTempCaseFiles([FromUri]int caseId)
        {
            //todo: make async
            _userTemporaryFilesStorage.ResetCacheForObject(caseId);
            return Ok();
        }

        #region Private Methods

        private string GetBasePath(int customerId)
        {
            return _settingsLogic.GetFilePath(customerId);
        }

        #endregion
    }
}