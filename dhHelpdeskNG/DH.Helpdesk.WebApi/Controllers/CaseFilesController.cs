using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseFilesController : BaseApiController
    {
        private readonly ICaseFileService _caseFileService;

        public CaseFilesController(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
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
        public  Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int fileId, [FromUri]int cid, bool? inline = false)
        {
            var fileContent = _caseFileService.GetCaseFile(cid, caseId, fileId, true); //TODO: async
        
            IHttpActionResult res = new FileResult(fileContent.FileName, fileContent.Content, Request, inline ?? false);
            return Task.FromResult(res);
        }

    }
}