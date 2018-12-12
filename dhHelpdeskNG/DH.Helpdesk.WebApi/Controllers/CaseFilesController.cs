using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Tools;
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
        private readonly ITemporaryFilesCacheFactory _userTemporaryFilesStorageFactory;
        private readonly ICaseService _caseService;
        private readonly ISettingsLogic _settingsLogic;

        #region ctor()

        public CaseFilesController(ICaseService caseService,
            ICaseFileService caseFileService,
            ISettingsLogic settingsLogic,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory)
        {
            _settingsLogic = settingsLogic;
            _caseService = caseService;
            _caseFileService = caseFileService;
            _userTemporaryFilesStorageFactory = userTemporaryFilesStorageFactory;
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
        [Route("{caseKey}/uploadfile")]
        [SkipCustomerAuthorization]
        public async Task<IHttpActionResult> UploadFile([FromUri]string caseKey)
        {
            if (string.IsNullOrEmpty(caseKey))
                return BadRequest("caseKey parameter is null or empty");

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //await Task.Delay(TimeSpan.FromSeconds(10));

            int caseId;
            var now = DateTime.Now;
            var topic = ModuleName.Cases;
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            try
            {
                if (GuidHelper.IsGuid(caseKey))
                {
                    var tempStorage = _userTemporaryFilesStorageFactory.CreateForModule(topic);
                    foreach (var stream in filesReadToProvider.Contents)
                    {
                        var fileBytes = await stream.ReadAsByteArrayAsync();
                        var fileName = stream.Headers.ContentDisposition.FileName.Unquote();

                        //fix name
                        if (tempStorage.FileExists(fileName, caseKey, topic))
                            fileName = $"{now}-{fileName}";

                        tempStorage.AddFile(fileBytes, fileName, caseKey, topic);
                    }
                }
                else if (Int32.TryParse(caseKey, out caseId))
                {
                    var customerId = _caseService.GetCaseCustomerId(caseId);
                    var basePath = _settingsLogic.GetFilePath(customerId);

                    foreach (var stream in filesReadToProvider.Contents)
                    {
                        var fileBytes = await stream.ReadAsByteArrayAsync();
                        var fileName = stream.Headers.ContentDisposition.FileName.Unquote();

                        if (_caseFileService.FileExists(caseId, fileName))
                            fileName = $"{now}-{fileName}";

                        var caseFileDto = new CaseFileDto(
                            fileBytes,
                            basePath,
                            fileName,
                            now,
                            caseId,
                            UserId);

                        _caseFileService.AddFile(caseFileDto);
                    }
                }

                return Ok(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); //todo: check how to return an error
            }
        }
    }
}