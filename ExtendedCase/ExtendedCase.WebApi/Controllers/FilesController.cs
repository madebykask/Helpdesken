using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ExtendedCase.Common.Enums;
using ExtendedCase.Logic.Services;
using ExtendedCase.Logic.Utils.Files;
using ExtendedCase.Models.Files;
using ExtendedCase.WebApi.Infrastructure.Results;

namespace ExtendedCase.WebApi.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        private readonly IGlobalSettingsService _globalSettingService;
        private readonly ICaseFileService _caseFileService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;
        private readonly ISettingsService _settingsService;

        /// <summary>
        /// This file was copy pasted from DH.Helpdesk.WebApi - CaseFilesController. Because DH.Helpdesk.WebApi has authentication, but this one don't.
        /// When common auth is implemented, use DH.Helpdesk.WebApi controller instead
        /// </summary>
        /// <param name="globalSettingService"></param>
        /// <param name="caseFileService"></param>
        /// <param name="settingsService"></param>
        public FilesController(IGlobalSettingsService globalSettingService, 
            ICaseFileService caseFileService,
            ISettingsService settingsService)
        {
            _globalSettingService = globalSettingService;
            _caseFileService = caseFileService;
            _settingsService = settingsService;
            _userTemporaryFilesStorage = new TemporaryFilesCache(ModuleName.Cases);
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
                var fileName = stream.Headers.ContentDisposition.FileName.Trim().Trim('"').Trim();

                var extension = Path.GetExtension(fileName);

                if (!_globalSettingService.IsExtensionInWhitelist(extension))
                    throw new HttpResponseException(HttpStatusCode.Forbidden);

                if (_userTemporaryFilesStorage.FileExists(fileName, caseKey.ToString()))
                    fileName = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} - {fileName}";

                _userTemporaryFilesStorage.AddFile(fileBytes, fileName, caseKey.ToString());
                return Ok(fileName);
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpPost]
        [Route("{caseId:int}/file")] // remember to update WebApiCorsPolicyProvider if url is changed
        public async Task<IHttpActionResult> UploadCaseFile([FromUri]int caseId, [FromUri]int cid, [FromUri]int caseNumber, [FromUri]string userName)
        {
            var now = DateTime.Now;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var customerId = cid; // in that case "case customerid" equals "current customerid" - no need to get case customerId
            var basePath = GetBasePath(customerId);

            var stream = filesReadToProvider.Contents.FirstOrDefault();

            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Trim().Trim('"').Trim();

                var extension = Path.GetExtension(fileName);

                if (!_globalSettingService.IsExtensionInWhitelist(extension))
                    throw new HttpResponseException(HttpStatusCode.Forbidden);

                if (_caseFileService.FileExists(caseId, fileName))
                    fileName = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} - {fileName}";

                var caseFileDto = new CaseFileDto(
                    fileBytes,
                    basePath,
                    fileName,
                    now,
                    caseId,
                    null);

                var fileId = _caseFileService.AddFile(caseFileDto, caseNumber);

                return Ok(new { id = fileId, name = fileName });
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpGet]
        [Route("{caseId:int}/file/{fileId:int}")] //ex: /api/Case/123/File/1203?cid=1&caseNumber=zzzz
        public Task<IHttpActionResult> DownloadExistingFile([FromUri] int caseId, [FromUri] int fileId, [FromUri] int cid, [FromUri] string caseNumber, bool? inline = false)
        {
            var fileContent = _caseFileService.GetCaseFile(cid, caseId, fileId, caseNumber, true); //TODO: async

            IHttpActionResult res = new FileResult(fileContent.FileName, fileContent.Content, Request, inline ?? false);

            //if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
            //{
            //    _fileViewLogService.Log(caseId, UserId, fileContent.FileName.Trim(), fileContent.FilePath, FileViewLogFileSource.WebApi,
            //        FileViewLogOperation.View);
            //}
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

        [HttpDelete]
        [Route("{caseKey:guid}/file")]
        public IHttpActionResult DeleteNewCaseFile([FromUri]Guid caseKey, [FromUri]string fileName)
        {
            var fileNameSafe = (fileName ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(fileNameSafe))
            {
                _userTemporaryFilesStorage.DeleteFile(fileNameSafe, caseKey.ToString());
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{caseId:int}/file/{fileId:int}")]
        public async Task<IHttpActionResult> DeleteCaseFile(int caseId, int fileId, [FromUri]int cid, [FromUri]int caseNumber, [FromUri]string fileName)
        {
            var customerId = cid;
            var basePath = GetBasePath(customerId);

            //var caseFileInfo = _caseFileService.GetCaseFile(caseId, fileId);
            _caseFileService.DeleteByCaseIdAndFileName(caseId, basePath, fileName, caseNumber);

            //if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
            //{
            //    var path = _filesStorage.ComposeFilePath(ModuleName.Cases, decimal.ToInt32(c.CaseNumber), basePath, "");
            //    _fileViewLogService.Log(caseId, UserId, caseFileInfo.FileName, path, FileViewLogFileSource.WebApi,
            //        FileViewLogOperation.Delete);
            //}
            //todo: ?
            //_invoiceArticleService.DeleteFileByCaseId(int.Parse(id), fileName.Trim());

            //IDictionary<string, string> errors;
            //var adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //_caseService.SaveFileDeleteHistory(c, caseFileInfo.FileName, UserId, adUser, out errors);

            //todo: return  errors?
            return Ok();
        }

        #region Private Methods

        private string GetBasePath(int customerId)
        {
            var customerFilePath =_settingsService.GetFilePath(customerId);
            if (string.IsNullOrEmpty(customerFilePath))
                return _globalSettingService.GetAttachedFileFolder() ?? string.Empty;

            return customerFilePath;
        }

        #endregion
    }
}
