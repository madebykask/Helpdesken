using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseLogsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogService _caselLogService;
        private readonly IMapper _mapper;

        public CaseLogsController(IUserService userService, ILogService caselLogService, IMapper mapper)
        {
            _userService = userService;
            _caselLogService = caselLogService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{caseId:int}/logs")]
        public async Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int cid)
        {
            var currentUser = _userService.GetUser(UserId);
            var includeInternalLogs = currentUser.CaseInternalLogPermission.ToBool();
            var logEntities = await _caselLogService.GetLogsByCaseIdAsync(caseId, includeInternalLogs).ConfigureAwait(false);

            var model = _mapper.Map<List<CaseLogOutputModel>>(logEntities);

            return Ok(model);
        }
    }
}