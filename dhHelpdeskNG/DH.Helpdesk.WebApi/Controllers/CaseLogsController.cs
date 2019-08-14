using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseLogsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogService _caseLogService;
        private readonly IMapper _mapper;
        private ICaseFieldSettingService _caseFieldSettingService;
        private ICaseFieldSettingsHelper _caseFieldSettingsHelper;


        public CaseLogsController(
            IUserService userService, 
            ILogService caseLogService, 
            ICaseFieldSettingService caseFieldSettingService,
            ICaseFieldSettingsHelper caseFieldSettingsHelper,
            IMapper mapper)
        {
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _caseFieldSettingService = caseFieldSettingService;
            _userService = userService;
            _caseLogService = caseLogService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{caseId:int}/logs")]
        [CheckUserCasePermissions(CaseIdParamName = "caseId", CheckBody = true)]
        public async Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int cid)
        {
            var currentUser = await _userService.GetUserAsync(UserId);
            var includeInternalLogs = currentUser.CaseInternalLogPermission.ToBool();

            var fieldName = _caseFieldSettingsHelper.GetFieldName(GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal.ToString());
            var isTwoAttachmentsMode = _caseFieldSettingService.GetCaseFieldSetting(cid, fieldName)?.ShowExternal.ToBool() ?? false;


            var logEntities = 
                await _caseLogService.GetLogsByCaseIdAsync(caseId, includeInternalLogs, includeInternalLogs && isTwoAttachmentsMode).ConfigureAwait(false);

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