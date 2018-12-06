using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseFilesController : BaseApiController
    {
        private readonly ICaseFileService _caseFileService;
        private readonly ITemporaryFilesCacheFactory _userTemporaryFilesStorageFactory;

        public CaseFilesController(ICaseFileService caseFileService,
                                   ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory)
        {
            _caseFileService = caseFileService;
            _userTemporaryFilesStorageFactory = userTemporaryFilesStorageFactory;
        }

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
        [Route("uploadfiles")]
        public async Task<IHttpActionResult> UploadFiles(/*string key*/)
        {
            var tempStorage = _userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    //Do whatever you want with filename and its binary data.

                    tempStorage.AddFile(buffer, fileName, Guid.NewGuid().ToString(), ModuleName.Cases);
                }
             
                return Ok(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message); //todo: check how to return an error
            }
        }
    }
}