using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseLogsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogService _caselLogService;
        private readonly IMapper _mapper;
        private readonly ISettingsLogic _settingsLogic;
        private readonly ILogFileService _logFileService;

        public CaseLogsController(
            IUserService userService, 
            ILogService caselLogService, 
            ILogFileService logFileService,
            IMapper mapper, 
            ISettingsLogic settingsLogic)
        {
            _logFileService = logFileService;
            _userService = userService;
            _caselLogService = caselLogService;
            _mapper = mapper;
            _settingsLogic = settingsLogic;
        }

        [HttpGet]
        [Route("{caseId:int}/logs")]
        public async Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int cid)
        {
            var currentUser = _userService.GetUser(UserId);
            var includeInternalLogs = currentUser.CaseInternalLogPermission.ToBool();

            var logEntities = await _caselLogService.GetLogsByCaseIdAsync(caseId, includeInternalLogs).ConfigureAwait(false);
            
            //todo: log files
            //var exLogFiles = _logFileService.GetLogFilesByCaseId(caseId).Select(x => new CaseAttachedExFileModel(x.Id, x.Name, LogId = x.ObjId));
            var model = _mapper.Map<List<CaseLogOutputModel>>(logEntities);

            return Ok(model);
        }

        //ex: /api/Case/123/LogFile/1203?cid=1
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}/logfile/{fileId:int}")] 
        public Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int fileId, [FromUri]int cid, bool? inline = false)
        {
            var basePath = _settingsLogic.GetFilePath(cid);
            var logFile = _logFileService.GetFileContentById(fileId, basePath);
            if (logFile == null)
            {
                SendResponse("The log file was not found", HttpStatusCode.NotFound);
            }

            IHttpActionResult res = new FileResult(logFile.FileName, logFile.Content, Request, inline ?? false);
            return Task.FromResult(res);
        }
    }
}