using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
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

        #region ctor()

        public CaseFilesController(ICaseService caseService,
            ICaseFileService caseFileService,
            ISettingsLogic settingsLogic,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory)
        {
            _settingsLogic = settingsLogic;
            _caseService = caseService;
            _caseFileService = caseFileService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
        }

        #endregion

        /// <summary>
        /// Get files content. Used to download files.
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="fileId"></param>
        /// <param name="cid"></param>
        /// <returns>File</returns>
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}/File/{fileId:int}")] //ex: /api/Case/123/File/1203?cid=1
        public Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int fileId, [FromUri]int cid, bool? inline = false)
        {
            var fileContent = _caseFileService.GetCaseFile(cid, caseId, fileId, true); //TODO: async
        
            IHttpActionResult res = new FileResult(fileContent.FileName, fileContent.Content, Request, inline ?? false);
            return Task.FromResult(res);
        }

        [HttpPost]
        [Route("{caseKey:guid}/file")] // remember to update WebApiCorsPolicyProvider if url is changed
        [SkipCustomerAuthorization] // ignore check for new case
        public async Task<IHttpActionResult> UploadNewCaseFile([FromUri] Guid caseKey)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var now = DateTime.Now;
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var stream = filesReadToProvider.Contents.FirstOrDefault();
            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Unquote().Trim();

                //fix name
                if (_userTemporaryFilesStorage.FileExists(fileName, caseKey.ToString()))
                {
                    fileName = $"{now}-{fileName}"; // handle on the client file name change
                }

                _userTemporaryFilesStorage.AddFile(fileBytes, fileName, caseKey.ToString());
                return Ok(fileName);
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpPost]
        [Route("{caseKey:int}/file")] // remember to update WebApiCorsPolicyProvider if url is changed
        public async Task<IHttpActionResult> UploadCaseFile([FromUri]int caseKey)
        {
            var now = DateTime.Now;
            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            
            var customerId = _caseService.GetCaseCustomerId(caseKey);
            var basePath = GetBasePath(customerId);

            var stream = filesReadToProvider.Contents.FirstOrDefault();

            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Unquote().Trim();

                if (_caseFileService.FileExists(caseKey, fileName))
                {
                    fileName = $"{now}-{fileName}"; // handle on the client filename change
                }

                var caseFileDto = new CaseFileDto(
                    fileBytes,
                    basePath,
                    fileName,
                    now,
                    caseKey,
                    UserId);

                var fileId = _caseFileService.AddFile(caseFileDto);
                return Ok(new { id = fileId, name = fileName});
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpDelete]
        [Route("{caseKey:guid}/file")]
        [SkipCustomerAuthorization] //skip check for new case
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
        [Route("{caseKey:int}/file/{fileId:int}")]
        [CheckUserPermissions(UserPermission.DeleteAttachedFilePermission)]
        public IHttpActionResult DeleteCaseFile(int caseKey, int fileId)
        {
            //todo: make Async

            var c = _caseService.GetCaseById(caseKey);
            var customerId = c.Customer_Id;
            var basePath = GetBasePath(customerId);

            var caseFileInfo = _caseFileService.GetCaseFile(caseKey, fileId);
            _caseFileService.DeleteByCaseIdAndFileName(caseKey, basePath, caseFileInfo.FileName);

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