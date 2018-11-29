using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseSaveController : BaseCaseController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseLockService _caseLockService;

        public CaseSaveController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService, ICaseLockService caseLockService)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseLockService = caseLockService;
        }

        [HttpPost]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("save/{caseId:int}")]
        public async Task<IHttpActionResult> Post([FromUri] int? caseId, [FromUri]int cid, [FromBody]CaseEditInputModel model)
        {
            if (!caseId.HasValue)
            {
                return BadRequest("Creating new case is not supported.");
            }

            var lockData = await _caseLockService.GetCaseLockAsync(caseId.Value);
            if (lockData != null && lockData.UserId != UserId)
            {
                return BadRequest($"Case is locked by {lockData.User.UserID}.");
            }

            //TODO: check if user can edit case
            //TODO: validate input
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var currentCase = caseId.HasValue ? _caseService.GetCaseById(caseId.Value) : new Domain.Case();

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
                currentCase.CaseResponsibleUser_Id = model.ResponsibleUserId;

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
                currentCase.Performer_User_Id = model.PerformerId;

            CaseLog caseLog = null; // TODO: implement

            var leadTime = 0;// TODO: add calculation
            var actionLeadTime = 0;// TODO: add calculation
            var actionExternalTime = 0;// TODO: add calculation

            var ei = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.WebApi,
                LeadTimeForNow = leadTime,
                ActionLeadTime = actionLeadTime,
                ActionExternalTime = actionExternalTime
            };

            IDictionary<string, string> errors;
            var caseHistoryId = this._caseService.SaveCase(
                currentCase,
                caseLog,
                UserId,
                UserName,
                ei,
                out errors,
                null,  // TODO: Parentcase
                null); //TODO: FollowerUsers

            //TODO: History 

            return Ok();
        }
    }
}
