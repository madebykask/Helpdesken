using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Logs;
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
            var isTwoAttachmentsMode = _caseFieldSettingService.GetCaseFieldSetting(cid, fieldName)?.IsActive ?? false;

            var includeInternalFiles = includeInternalLogs && isTwoAttachmentsMode;
            var logEntities = 
                await _caseLogService.GetLogsByCaseIdAsync(caseId, includeInternalLogs, includeInternalFiles).ConfigureAwait(false);

            var model = MapLogsToModel(logEntities, includeInternalFiles);
            return Ok(model);
        }
        
        private IList<CaseLogOutputModel> MapLogsToModel(IList<CaseLogData> logs, bool isTwoAttachmentMode)
        {
            var items = new List<CaseLogOutputModel>();

            foreach (var log in logs)
            {
                var internalText = log.InternalText;
                var files = log.Files ?? new List<LogFileData>();
                var internalFiles = files.Where(f => f.LogType == LogFileType.Internal).ToList();
                var externalFiles = files.Where(f => f.LogType == LogFileType.External).ToList();

                //create two external and internal items out of one if has both properties
                if (!string.IsNullOrEmpty(log.ExternalText) && !string.IsNullOrEmpty(log.InternalText) || 
                    (isTwoAttachmentMode && internalFiles.Any() && externalFiles.Any()))
                {
                    //create external log item
                    log.InternalText = null;
                    log.Files = externalFiles;
                    var itemModel = CreateCaseLogOutputModel(log);
                    items.Add(itemModel);

                    //create internal
                    log.ExternalText = null;
                    log.InternalText = internalText;
                    log.Files = isTwoAttachmentMode ? internalFiles : null;
                    itemModel = CreateCaseLogOutputModel(log);
                    items.Add(itemModel);
                }
                else
                {
                    //check if internal log files should be included for internal log only if 2 attachments mode is enabled
                    var isInternalLog = !string.IsNullOrEmpty(log.InternalText);
                    if (isInternalLog && isTwoAttachmentMode)
                    {
                        log.Files = files.Where(f => f.LogType == LogFileType.Internal).ToList();
                    }
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