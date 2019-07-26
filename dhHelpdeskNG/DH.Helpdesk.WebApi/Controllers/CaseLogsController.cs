using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Models.Case.Logs;
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
    public class CaseLogsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogService _caseLogService;
        private readonly IMapper _mapper;
        private readonly ISettingsLogic _settingsLogic;
        private readonly ILogFileService _logFileService;
        private readonly ICaseFileService _caseFileService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;


        public CaseLogsController(
            IUserService userService, 
            ILogService caseLogService, 
            ILogFileService logFileService,
            ICaseFileService caseFileService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IMapper mapper, 
            ISettingsLogic settingsLogic)
        {
            _caseFileService = caseFileService;
            _logFileService = logFileService;
            _userService = userService;
            _caseLogService = caseLogService;
            _mapper = mapper;
            _settingsLogic = settingsLogic;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
        }

        [HttpGet]
        [Route("{caseId:int}/logs")]
        [CheckUserCasePermissions(CaseIdParamName = "caseId", CheckBody = true)]
        public async Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int cid)
        {
            var currentUser = await _userService.GetUserAsync(UserId);
            var includeInternalLogs = currentUser.CaseInternalLogPermission.ToBool();

            var logEntities = await _caseLogService.GetLogsByCaseIdAsync(caseId, includeInternalLogs).ConfigureAwait(false);

            var model = MapLogsToModel(logEntities);
            return Ok(model);
        }
        
        private IList<CaseLogOutputModel> MapLogsToModel(IList<CaseLogData> logs)
        {
            var items = new List<CaseLogOutputModel>();

            foreach (var log in logs)
            {
                //create two external and internal items out of one if has both properties
                if (!string.IsNullOrEmpty(log.ExternalText) && !string.IsNullOrEmpty(log.InternalText))
                {
                    var internalText = log.InternalText;

                    //create external log item
                    log.InternalText = null;
                    var itemModel = CreateCaseLogOutputModel(log);
                    items.Add(itemModel);

                    //create internal
                    log.ExternalText = null;
                    log.InternalText = internalText;

                    itemModel = CreateCaseLogOutputModel(log);
                    items.Add(itemModel);
                }
                else
                {
                    var itemModel = CreateCaseLogOutputModel(log);
                    items.Add(itemModel);
                }
            }

            return items;
        }

        private CaseLogOutputModel CreateCaseLogOutputModel(CaseLogData log)
        {
            var itemModel = _mapper.Map<CaseLogOutputModel>(log);
            return itemModel;
        }
    }
}